#pragma warning disable SA1600
namespace RxBim.Logs.Settings.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using Assemblies;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;
    using Serilog;
    using Serilog.Configuration;
    using Serilog.Core;
    using Serilog.Debugging;
    using Serilog.Events;

    internal class ConfigurationReader : IConfigurationReader
    {
        private const string LevelSwitchNameRegex = @"^\$[A-Za-z]+[A-Za-z0-9]*$";

        private readonly IConfigurationSection _section;
        private readonly IReadOnlyCollection<Assembly> _configurationAssemblies;
        private readonly ResolutionContext _resolutionContext;

        public ConfigurationReader(
            IConfigurationSection configSection,
            AssemblyFinder assemblyFinder,
            IConfiguration? configuration = null)
        {
            _section = configSection ?? throw new ArgumentNullException(nameof(configSection));
            _configurationAssemblies = LoadConfigurationAssemblies(_section, assemblyFinder);
            _resolutionContext = new ResolutionContext(configuration);
        }

        // Used internally for processing nested configuration sections -- see GetMethodCalls below.
        internal ConfigurationReader(
            IConfigurationSection? configSection,
            IReadOnlyCollection<Assembly> configurationAssemblies,
            ResolutionContext resolutionContext)
        {
            _section = configSection ?? throw new ArgumentNullException(nameof(configSection));
            _configurationAssemblies = configurationAssemblies ??
                                       throw new ArgumentNullException(nameof(configurationAssemblies));
            _resolutionContext = resolutionContext ?? throw new ArgumentNullException(nameof(resolutionContext));
        }

        public void Configure(LoggerConfiguration loggerConfiguration)
        {
            ProcessLevelSwitchDeclarations();

            ApplyMinimumLevel(loggerConfiguration);
            ApplyEnrichment(loggerConfiguration);
            ApplyFilters(loggerConfiguration);
            ApplyDestructuring(loggerConfiguration);
            ApplySinks(loggerConfiguration);
            ApplyAuditSinks(loggerConfiguration);
        }

        void IConfigurationReader.ApplyEnrichment(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration)
        {
            var methodCalls = GetMethodCalls(_section);
            CallConfigurationMethods(methodCalls,
                FindEventEnricherConfigurationMethods(_configurationAssemblies),
                loggerEnrichmentConfiguration);
        }

        void IConfigurationReader.ApplySinks(LoggerSinkConfiguration loggerSinkConfiguration)
        {
            var methodCalls = GetMethodCalls(_section);
            CallConfigurationMethods(methodCalls,
                FindSinkConfigurationMethods(_configurationAssemblies),
                loggerSinkConfiguration);
        }

        internal static IConfigurationArgumentValue GetArgumentValue(
            IConfigurationSection? argumentSection,
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            // Reject configurations where an element has both scalar and complex
            // values as a result of reading multiple configuration sources.
            if (argumentSection?.Value != null && argumentSection.GetChildren().Any())
            {
                throw new InvalidOperationException(
                    $"The value for the argument '{argumentSection.Path}' is assigned different value " +
                    "types in more than one configuration source. Ensure all configurations consistently " +
                    "use either a scalar (int, string, boolean) or a complex (array, section, list, " +
                    "POCO, etc.) type for this argument value.");
            }

            IConfigurationArgumentValue argumentValue = argumentSection?.Value != null
                ? new StringArgumentValue(argumentSection.Value)
                : new ObjectArgumentValue(argumentSection, configurationAssemblies);

            return argumentValue;
        }

        private static MethodInfo? SelectConfigurationMethod(
            IEnumerable<MethodInfo> candidateMethods,
            string name,
            IEnumerable<string> suppliedArgumentNames)
        {
            // Per issue #111, it is safe to use case-insensitive matching on argument names. The CLR doesn't permit this type
            // of overloading, and the Microsoft.Extensions.Configuration keys are case-insensitive (case is preserved with some
            // config sources, but key-matching is case-insensitive and case-preservation does not appear to be guaranteed).
            var candidateMethodsList = candidateMethods.ToList();
            var selectedMethod = candidateMethodsList
                .Where(m => m.Name == name)
                .Where(m => m.GetParameters()
                    .Skip(1)
                    .All(p => HasImplicitValueWhenNotSpecified(p) ||
                              ParameterNameMatches(p.Name, suppliedArgumentNames)))
                .OrderByDescending(m =>
                {
                    var matchingArgs = m.GetParameters().Where(p => ParameterNameMatches(p.Name, suppliedArgumentNames))
                        .ToList();

                    // Prefer the configuration method with most number of matching arguments and of those the ones with
                    // the most string type parameters to predict best match with least type casting
                    return new Tuple<int, int>(
                        matchingArgs.Count,
                        matchingArgs.Count(p => p.ParameterType == typeof(string)));
                })
                .FirstOrDefault();

            if (selectedMethod is not null)
                return selectedMethod;

            var methodsByName = candidateMethodsList
                .Where(m => m.Name == name)
                .Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Skip(1).Select(p => p.Name))})")
                .ToList();

            if (!methodsByName.Any())
            {
                SelfLog.WriteLine(
                    $"Unable to find a method called {name}. Candidate methods are:{Environment.NewLine}{string.Join(Environment.NewLine, candidateMethodsList)}");
            }
            else
            {
                var suppliedArgumentNamesList = suppliedArgumentNames.ToList();
                SelfLog.WriteLine($"Unable to find a method called {name} "
                                  + (suppliedArgumentNamesList.Any()
                                      ? "for supplied arguments: " + string.Join(", ", suppliedArgumentNamesList)
                                      : "with no supplied arguments")
                                  + ". Candidate methods are:"
                                  + Environment.NewLine
                                  + string.Join(Environment.NewLine, methodsByName));
            }

            return selectedMethod;
        }

        private static bool IsValidSwitchName(string input)
        {
            return Regex.IsMatch(input, LevelSwitchNameRegex);
        }

        private static IReadOnlyCollection<Assembly> LoadConfigurationAssemblies(
            IConfiguration section,
            AssemblyFinder assemblyFinder)
        {
            var serilogAssembly = typeof(ILogger).Assembly;
            var assemblies = new Dictionary<string, Assembly> { [serilogAssembly.FullName] = serilogAssembly };

            var usingSection = section.GetSection("Using");
            if (usingSection.GetChildren().Any())
            {
                foreach (var simpleName in usingSection.GetChildren().Select(c => c.Value))
                {
                    if (string.IsNullOrWhiteSpace(simpleName))
                    {
                        throw new InvalidOperationException(
                            "A zero-length or whitespace assembly name was supplied to a Serilog.Using configuration statement.");
                    }

                    var assembly = Assembly.Load(new AssemblyName(simpleName));
                    if (!assemblies.ContainsKey(assembly.FullName))
                        assemblies.Add(assembly.FullName, assembly);
                }
            }

            foreach (var assemblyName in assemblyFinder.FindAssembliesContainingName("serilog"))
            {
                var assumed = Assembly.Load(assemblyName);
                if (assumed != null && !assemblies.ContainsKey(assumed.FullName))
                    assemblies.Add(assumed.FullName, assumed);
            }

            return assemblies.Values.ToList().AsReadOnly();
        }

        private static bool HasImplicitValueWhenNotSpecified(ParameterInfo paramInfo)
        {
            return paramInfo.HasDefaultValue

                   // parameters of type IConfiguration are implicitly populated with provided Configuration
                   || paramInfo.ParameterType == typeof(IConfiguration);
        }

        private static bool ParameterNameMatches(string actualParameterName, string suppliedName)
        {
            return suppliedName.Equals(actualParameterName, StringComparison.OrdinalIgnoreCase);
        }

        private static bool ParameterNameMatches(string actualParameterName, IEnumerable<string> suppliedNames)
        {
            return suppliedNames.Any(s => ParameterNameMatches(actualParameterName, s));
        }

        private static IList<MethodInfo> FindSinkConfigurationMethods(
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            var found = FindConfigurationExtensionMethods(configurationAssemblies, typeof(LoggerSinkConfiguration));
            if (configurationAssemblies.Contains(typeof(LoggerSinkConfiguration).GetTypeInfo().Assembly))
                found.AddRange(SurrogateConfigurationMethods.WriteTo);

            return found;
        }

        private static IList<MethodInfo> FindAuditSinkConfigurationMethods(
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            var found = FindConfigurationExtensionMethods(configurationAssemblies,
                typeof(LoggerAuditSinkConfiguration));
            if (configurationAssemblies.Contains(typeof(LoggerAuditSinkConfiguration).GetTypeInfo().Assembly))
                found.AddRange(SurrogateConfigurationMethods.AuditTo);
            return found;
        }

        private static IList<MethodInfo> FindFilterConfigurationMethods(
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            var found = FindConfigurationExtensionMethods(configurationAssemblies, typeof(LoggerFilterConfiguration));
            if (configurationAssemblies.Contains(typeof(LoggerFilterConfiguration).GetTypeInfo().Assembly))
                found.AddRange(SurrogateConfigurationMethods.Filter);

            return found;
        }

        private static IList<MethodInfo> FindDestructureConfigurationMethods(
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            var found = FindConfigurationExtensionMethods(configurationAssemblies,
                typeof(LoggerDestructuringConfiguration));
            if (configurationAssemblies.Contains(typeof(LoggerDestructuringConfiguration).GetTypeInfo().Assembly))
                found.AddRange(SurrogateConfigurationMethods.Destructure);

            return found;
        }

        private static IList<MethodInfo> FindEventEnricherConfigurationMethods(
            IReadOnlyCollection<Assembly> configurationAssemblies)
        {
            var found = FindConfigurationExtensionMethods(configurationAssemblies,
                typeof(LoggerEnrichmentConfiguration));
            if (configurationAssemblies.Contains(typeof(LoggerEnrichmentConfiguration).GetTypeInfo().Assembly))
                found.AddRange(SurrogateConfigurationMethods.Enrich);

            return found;
        }

        private static List<MethodInfo> FindConfigurationExtensionMethods(
            IEnumerable<Assembly> configurationAssemblies,
            Type configType)
        {
            return configurationAssemblies
                .SelectMany(a => a.ExportedTypes
                    .Select(t => t.GetTypeInfo())
                    .Where(t => t.IsSealed && t is { IsAbstract: true, IsNested: false }))
                .SelectMany(t => t.DeclaredMethods)
                .Where(m => m.IsStatic && m.IsPublic && m.IsDefined(typeof(ExtensionAttribute), false))
                .Where(m => m.GetParameters()[0].ParameterType == configType)
                .ToList();
        }

        private static LogEventLevel ParseLogEventLevel(string value)
        {
            if (!Enum.TryParse(value, out LogEventLevel parsedLevel))
                throw new InvalidOperationException($"The value {value} is not a valid Serilog level.");
            return parsedLevel;
        }

        private ILookup<string, Dictionary<string, IConfigurationArgumentValue>> GetMethodCalls(
            IConfiguration directive)
        {
            var children = directive.GetChildren().ToList();

            var result =
                (from child in children
                    where child.Value != null // Plain string
                    select new { Name = child.Value, Args = new Dictionary<string, IConfigurationArgumentValue>() })
                .Concat(
                    (from child in children
                        where child.Value == null
                        let name = GetSectionName(child)
                        let callArgs = (from argument in child.GetSection("Args").GetChildren()
                            select new
                            {
                                Name = argument.Key,
                                Value = GetArgumentValue(argument, _configurationAssemblies)
                            }).ToDictionary(p => p.Name, p => p.Value)
                        select new { Name = name, Args = callArgs }))
                .ToLookup(p => p.Name, p => p.Args);

            return result;

            static string GetSectionName(IConfiguration s)
            {
                var name = s.GetSection("Name");
                if (name.Value == null)
                {
                    throw new InvalidOperationException(
                        $"The configuration value in {name.Path} has no 'Name' element.");
                }

                return name.Value;
            }
        }

        private void ProcessLevelSwitchDeclarations()
        {
            var levelSwitchesDirective = _section.GetSection("LevelSwitches");
            foreach (var levelSwitchDeclaration in levelSwitchesDirective.GetChildren())
            {
                var switchName = levelSwitchDeclaration.Key;
                var switchInitialLevel = levelSwitchDeclaration.Value;

                // switchName must be something like $switch to avoid ambiguities
                if (!IsValidSwitchName(switchName))
                {
                    throw new FormatException(
                        $"\"{switchName}\" is not a valid name for a Level Switch declaration. Level switch must be declared with a '$' sign, like \"LevelSwitches\" : {{\"$switchName\" : \"InitialLevel\"}}");
                }

                LoggingLevelSwitch newSwitch;
                if (string.IsNullOrEmpty(switchInitialLevel))
                {
                    newSwitch = new LoggingLevelSwitch();
                }
                else
                {
                    var initialLevel = ParseLogEventLevel(switchInitialLevel);
                    newSwitch = new LoggingLevelSwitch(initialLevel);
                }

                SubscribeToLoggingLevelChanges(levelSwitchDeclaration, newSwitch);

                // make them available later on when resolving argument values
                _resolutionContext.AddLevelSwitch(switchName, newSwitch);
            }
        }

        private void ApplyMinimumLevel(LoggerConfiguration loggerConfiguration)
        {
            var minimumLevelDirective = _section.GetSection("MinimumLevel");

            var defaultMinLevelDirective = minimumLevelDirective.Value != null
                ? minimumLevelDirective
                : minimumLevelDirective.GetSection("Default");
            if (defaultMinLevelDirective.Value != null)
            {
                ApplyMinimumLevelFunc(defaultMinLevelDirective,
                    (configuration, levelSwitch) => configuration.ControlledBy(levelSwitch));
            }

            var minLevelControlledByDirective = minimumLevelDirective.GetSection("ControlledBy");
            if (minLevelControlledByDirective.Value != null)
            {
                var globalMinimumLevelSwitch =
                    _resolutionContext.LookUpSwitchByName(minLevelControlledByDirective.Value);

                // not calling ApplyMinimumLevel local function because here we have a reference to a LogLevelSwitch already
                if (globalMinimumLevelSwitch != null)
                    loggerConfiguration.MinimumLevel.ControlledBy(globalMinimumLevelSwitch);
            }

            foreach (var overrideDirective in minimumLevelDirective.GetSection("Override").GetChildren())
            {
                var overridePrefix = overrideDirective.Key;
                var overridenLevelOrSwitch = overrideDirective.Value;
                if (Enum.TryParse(overridenLevelOrSwitch, out LogEventLevel _))
                {
                    ApplyMinimumLevelFunc(overrideDirective,
                        (configuration, levelSwitch) => configuration.Override(overridePrefix, levelSwitch));
                }
                else
                {
                    var overrideSwitch = _resolutionContext.LookUpSwitchByName(overridenLevelOrSwitch);

                    // not calling ApplyMinimumLevel local function because here we have a reference to a LogLevelSwitch already
                    if (overrideSwitch != null)
                        loggerConfiguration.MinimumLevel.Override(overridePrefix, overrideSwitch);
                }
            }

            return;

            void ApplyMinimumLevelFunc(
                IConfigurationSection directive,
                Action<LoggerMinimumLevelConfiguration, LoggingLevelSwitch> applyConfigAction)
            {
                var minimumLevel = ParseLogEventLevel(directive.Value);

                var levelSwitch = new LoggingLevelSwitch(minimumLevel);
                applyConfigAction(loggerConfiguration.MinimumLevel, levelSwitch);

                SubscribeToLoggingLevelChanges(directive, levelSwitch);
            }
        }

        private void SubscribeToLoggingLevelChanges(IConfigurationSection levelSection, LoggingLevelSwitch levelSwitch)
        {
            ChangeToken.OnChange(
                levelSection.GetReloadToken,
                () =>
                {
                    if (Enum.TryParse(levelSection.Value, out LogEventLevel minimumLevel))
                        levelSwitch.MinimumLevel = minimumLevel;
                    else
                        SelfLog.WriteLine($"The value {levelSection.Value} is not a valid Serilog level.");
                });
        }

        private void ApplyFilters(LoggerConfiguration loggerConfiguration)
        {
            var filterDirective = _section.GetSection("Filter");
            if (!filterDirective.GetChildren().Any())
                return;

            var methodCalls = GetMethodCalls(filterDirective);
            CallConfigurationMethods(methodCalls,
                FindFilterConfigurationMethods(_configurationAssemblies),
                loggerConfiguration.Filter);
        }

        private void ApplyDestructuring(LoggerConfiguration loggerConfiguration)
        {
            var destructureDirective = _section.GetSection("Destructure");
            if (!destructureDirective.GetChildren().Any())
                return;

            var methodCalls = GetMethodCalls(destructureDirective);
            CallConfigurationMethods(methodCalls,
                FindDestructureConfigurationMethods(_configurationAssemblies),
                loggerConfiguration.Destructure);
        }

        private void ApplySinks(LoggerConfiguration loggerConfiguration)
        {
            var writeToDirective = _section.GetSection("WriteTo");
            if (!writeToDirective.GetChildren().Any())
                return;

            var methodCalls = GetMethodCalls(writeToDirective);
            CallConfigurationMethods(methodCalls,
                FindSinkConfigurationMethods(_configurationAssemblies),
                loggerConfiguration.WriteTo);
        }

        private void ApplyAuditSinks(LoggerConfiguration loggerConfiguration)
        {
            var auditToDirective = _section.GetSection("AuditTo");
            if (!auditToDirective.GetChildren().Any())
                return;

            var methodCalls = GetMethodCalls(auditToDirective);
            CallConfigurationMethods(methodCalls,
                FindAuditSinkConfigurationMethods(_configurationAssemblies),
                loggerConfiguration.AuditTo);
        }

        private void ApplyEnrichment(LoggerConfiguration loggerConfiguration)
        {
            var enrichDirective = _section.GetSection("Enrich");
            if (enrichDirective.GetChildren().Any())
            {
                var methodCalls = GetMethodCalls(enrichDirective);
                CallConfigurationMethods(methodCalls,
                    FindEventEnricherConfigurationMethods(_configurationAssemblies),
                    loggerConfiguration.Enrich);
            }

            var propertiesDirective = _section.GetSection("Properties");
            if (!propertiesDirective.GetChildren().Any())
                return;

            foreach (var enrichPropertyDirective in propertiesDirective.GetChildren())
            {
                loggerConfiguration.Enrich.WithProperty(enrichPropertyDirective.Key, enrichPropertyDirective.Value);
            }
        }

        private void CallConfigurationMethods(
            ILookup<string, Dictionary<string, IConfigurationArgumentValue>> methods,
            IList<MethodInfo> configurationMethods,
            object receiver)
        {
            foreach (var method in methods.SelectMany(g => g.Select(x => new { g.Key, Value = x })))
            {
                var methodInfo = SelectConfigurationMethod(configurationMethods, method.Key, method.Value.Keys);

                if (methodInfo != null)
                {
                    var call = (from p in methodInfo.GetParameters().Skip(1)
                        let directive = method.Value.FirstOrDefault(s => ParameterNameMatches(p.Name, s.Key))
                        select directive.Key == null
                            ? GetImplicitValueForNotSpecifiedKey(p, methodInfo)
                            : directive.Value.ConvertTo(p.ParameterType, _resolutionContext)).ToList();

                    call.Insert(0, receiver);
                    methodInfo.Invoke(null, call.ToArray());
                }
            }
        }

        private object? GetImplicitValueForNotSpecifiedKey(ParameterInfo parameter, MethodInfo methodToInvoke)
        {
            if (!HasImplicitValueWhenNotSpecified(parameter))
            {
                throw new InvalidOperationException(
                    "GetImplicitValueForNotSpecifiedKey() should only be called for parameters for which HasImplicitValueWhenNotSpecified() is true. " +
                    "This means something is wrong in the Serilog.Settings.Configuration code.");
            }

            if (parameter.ParameterType == typeof(IConfiguration))
            {
                if (_resolutionContext.HasAppConfiguration)
                {
                    return _resolutionContext.AppConfiguration;
                }

                if (parameter.HasDefaultValue)
                {
                    return parameter.DefaultValue;
                }

                throw new InvalidOperationException(
                    "Trying to invoke a configuration method accepting a `IConfiguration` argument. " +
                    $"This is not supported when only a `IConfigSection` has been provided. (method '{methodToInvoke}')");
            }

            return parameter.DefaultValue;
        }
    }
}