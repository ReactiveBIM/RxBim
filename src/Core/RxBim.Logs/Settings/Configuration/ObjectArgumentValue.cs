#pragma warning disable SA1600
namespace RxBim.Logs.Settings.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Configuration;

    internal class ObjectArgumentValue : IConfigurationArgumentValue
    {
        private readonly IConfigurationSection _section;
        private readonly IReadOnlyCollection<Assembly> _configurationAssemblies;

        public ObjectArgumentValue(
            IConfigurationSection? section,
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            _section = section ?? throw new ArgumentNullException(nameof(section));

            // used by nested logger configurations to feed a new pass by ConfigurationReader
            _configurationAssemblies = configurationAssemblies ??
                                       throw new ArgumentNullException(nameof(configurationAssemblies));
        }

        public object? ConvertTo(Type toType, ResolutionContext resolutionContext)
        {
            // return the entire section for internal processing
            if (toType == typeof(IConfigurationSection))
                return _section;

            // process a nested configuration to populate an Action<> logger/sink config parameter?
            var typeInfo = toType.GetTypeInfo();
            if (typeInfo.IsGenericType &&
                typeInfo.GetGenericTypeDefinition() is { } genericType && genericType == typeof(Action<>))
            {
                var configType = typeInfo.GenericTypeArguments[0];
                IConfigurationReader configReader =
                    new ConfigurationReader(_section, _configurationAssemblies, resolutionContext);

                return configType switch
                {
                    _ when configType == typeof(LoggerConfiguration) => new Action<LoggerConfiguration>(configReader
                        .Configure),
                    _ when configType == typeof(LoggerSinkConfiguration) => new Action<LoggerSinkConfiguration>(
                        configReader.ApplySinks),
                    _ when configType == typeof(LoggerEnrichmentConfiguration) =>
                        new Action<LoggerEnrichmentConfiguration>(configReader.ApplyEnrichment),
                    _ => throw new ArgumentException(
                        $"Configuration resolution for Action<{configType.Name}> parameter type at the path {_section.Path} is not implemented.")
                };
            }

            if (toType.IsArray)
                return CreateArray();

            if (IsContainer(toType, out var elementType) && TryCreateContainer(out var result))
                return result;

            // MS Config binding can work with a limited set of primitive types and collections
            return _section.Get(toType);

            object CreateArray()
            {
                var arrayElementType = toType.GetElementType()!;
                var configurationElements = _section.GetChildren().ToArray();
                var array = Array.CreateInstance(arrayElementType, configurationElements.Length);
                for (var i = 0; i < configurationElements.Length; ++i)
                {
                    var argumentValue =
                        ConfigurationReader.GetArgumentValue(configurationElements[i], _configurationAssemblies);
                    var value = argumentValue.ConvertTo(arrayElementType, resolutionContext);
                    array.SetValue(value, i);
                }

                return array;
            }

            bool TryCreateContainer(out object? container)
            {
                container = null;

                if (toType.GetConstructor(Type.EmptyTypes) == null)
                    return false;

                // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#collection-initializers
                var addMethod = toType.GetMethods().FirstOrDefault(m =>
                    !m.IsStatic && m.Name == "Add" && m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == elementType);
                if (addMethod == null)
                    return false;

                var configurationSections = _section.GetChildren().ToArray();
                container = Activator.CreateInstance(toType);

                foreach (var section in configurationSections)
                {
                    var argumentValue =
                        ConfigurationReader.GetArgumentValue(section, _configurationAssemblies);
                    var value = argumentValue.ConvertTo(elementType!, resolutionContext);
                    addMethod.Invoke(container, new[] { value });
                }

                return true;
            }
        }

        private static bool IsContainer(Type type, out Type? elementType)
        {
            elementType = null;
            foreach (var iType in type.GetInterfaces())
            {
                if (!iType.IsGenericType || iType.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                    continue;

                elementType = iType.GetGenericArguments()[0];
                return true;
            }

            return false;
        }
    }
}