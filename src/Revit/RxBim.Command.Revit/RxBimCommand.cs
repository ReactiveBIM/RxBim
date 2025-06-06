namespace RxBim.Command.Revit
{
    using System;
#if NETCOREAPP
    using System.IO;
#endif
    using System.Linq;
    using System.Reflection;
#if NETCOREAPP
    using System.Runtime.Loader;
#endif
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

    /// <summary>
    /// Revit command.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public abstract class RxBimCommand : IExternalCommand, IExternalCommandAvailability
    {
#if NETCOREAPP
        /// <summary>
        /// Allows you to turn off plugin execution in separated context. Might be useful for debugging
        /// via Addin Manager.
        /// </summary>
        protected virtual bool RunInSeparatedContext => true;
#endif

        /// <inheritdoc />
        public Result Execute(
            ExternalCommandData commandData,
            ref string? message,
            ElementSet elements)
        {
            var type = GetType();
            var assembly = type.Assembly;

#if NETCOREAPP
            if (!PluginContext.IsCurrentContextDefault(type) || !RunInSeparatedContext)
                return ExecuteCommand(commandData, ref message, elements, assembly);

            // Attempt to find already exist context. If there is no exist context - create new.
            var appName = Path.GetFileNameWithoutExtension(assembly.Location);
            var existAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName?.Contains(appName) ?? false)
                .ToList();
            var existContext = existAssemblies.Select(AssemblyLoadContext.GetLoadContext)
                .FirstOrDefault(c => !AssemblyLoadContext.Default.Equals(c));
            if (existContext is PluginContext context)
            {
                var instance = context.CreateInstanceInContext(type);
                if (instance is IExternalCommand command)
                    return command.Execute(commandData, ref message, elements);
            }

            var commandInstance = PluginContext.CreateInstanceInNewContext(type);
            if (commandInstance is IExternalCommand externalCommand)
            {
                return externalCommand.Execute(commandData, ref message, elements);
            }
#endif

            return ExecuteCommand(commandData, ref message, elements, assembly);
        }

        /// <inheritdoc/>
        public virtual bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return applicationData.ActiveUIDocument?.Document != null;
        }

        private Result ExecuteCommand(ExternalCommandData commandData, ref string? message, ElementSet elements, Assembly assembly)
        {
            var serviceProvider = Configure(commandData, assembly);

            var commandResult = CallCommandMethod(serviceProvider);

            SetMessageAndElements(ref message, elements, commandResult, serviceProvider);
            return commandResult.MapResultToRevitResult();
        }

        private IServiceProvider Configure(ExternalCommandData commandData, Assembly assembly)
        {
            var di = new CommandDiConfigurator(this, commandData);
            di.Configure(assembly);
            return di.Build();
        }

        private PluginResult CallCommandMethod(IServiceProvider serviceProvider)
        {
            var methodCaller = serviceProvider.GetService<IMethodCaller<PluginResult>>();
            var commandResult = methodCaller.InvokeMethod(serviceProvider, Constants.ExecuteMethodName);
            return commandResult;
        }

        private void SetMessageAndElements(
            ref string? message,
            ElementSet elements,
            PluginResult commandResult,
            IServiceProvider serviceProvider)
        {
            if (!string.IsNullOrEmpty(commandResult.Message))
            {
                message = commandResult.Message;
            }

            if (!commandResult.ElementIds.Any())
                return;

            var doc = serviceProvider.GetService<Document>();
            foreach (var id in commandResult.ElementIds)
            {
#if RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023
                var elementId = new ElementId((int)id);
#else
                var elementId = new ElementId(id);
#endif
                var element = doc.GetElement(elementId);
                elements.Insert(element);
            }
        }
    }
}