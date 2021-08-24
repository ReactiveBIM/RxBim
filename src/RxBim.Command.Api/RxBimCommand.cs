namespace RxBim.Command.Api
{
    using System.Linq;
    using System.Reflection;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Di;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

    /// <summary>
    /// Команда Revit
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public abstract class RxBimCommand : IExternalCommand, IExternalCommandAvailability
    {
        /// <inheritdoc />
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            var assembly = GetType().Assembly;

            var di = Configure(commandData, assembly);

            var commandResult = CallCommandMethod(di);

            SetMessageAndElements(ref message, elements, commandResult, di);
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

        private PluginResult CallCommandMethod(CommandDiConfigurator di)
        {
            var methodCaller = di.Container.GetService<IMethodCaller<PluginResult>>();
            var commandResult = methodCaller.InvokeCommand(di.Container, Constants.ExecuteMethodName);
            return commandResult;
        }

        private void SetMessageAndElements(
            ref string message,
            ElementSet elements,
            PluginResult commandResult,
            CommandDiConfigurator di)
        {
            if (!string.IsNullOrEmpty(commandResult.Message))
            {
                message = commandResult.Message;
            }

            if (commandResult.ElementIds.Any())
            {
                var doc = di.Container.GetService<Document>();
                foreach (var id in commandResult.ElementIds)
                {
                    var element = doc.GetElement(new ElementId(id));
                    elements.Insert(element);
                }
            }
        }
    }
}