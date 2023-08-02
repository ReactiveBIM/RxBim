namespace RxBim.Command.Revit
{
    using System;
    using System.Linq;
    using System.Reflection;
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
        /// <inheritdoc />
        public Result Execute(
            ExternalCommandData commandData,
            ref string? message,
            ElementSet elements)
        {
            var assembly = GetType().Assembly;

            var di = Configure(commandData, assembly);

            using var provider = di.Services.BuildServiceProvider(false);
            var commandResult = CallCommandMethod(di, provider);

            SetMessageAndElements(ref message, elements, commandResult, provider);
            return commandResult.MapResultToRevitResult();
        }

        /// <inheritdoc/>
        public virtual bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return applicationData.ActiveUIDocument?.Document != null;
        }

        private CommandDiConfigurator Configure(ExternalCommandData commandData, Assembly assembly)
        {
            var di = new CommandDiConfigurator(this, commandData);
            di.Configure(assembly);
            return di;
        }

        private PluginResult CallCommandMethod(CommandDiConfigurator di, IServiceProvider provider)
        {
            var methodCaller = provider.GetRequiredService<IMethodCaller<PluginResult>>();
            var commandResult = methodCaller.InvokeMethod(di.Services, Constants.ExecuteMethodName);
            return commandResult;
        }

        private void SetMessageAndElements(
            ref string? message,
            ElementSet elements,
            PluginResult commandResult,
            IServiceProvider provider)
        {
            if (!string.IsNullOrEmpty(commandResult.Message))
            {
                message = commandResult.Message;
            }

            if (!commandResult.ElementIds.Any())
                return;

            var doc = provider.GetRequiredService<Document>();
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