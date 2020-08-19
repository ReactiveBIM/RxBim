namespace PikTools.Command.Api
{
    using System.Linq;
    using System.Reflection;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Di;

    /// <summary>
    /// Команда Revit
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public abstract class PikToolsCommand : IExternalCommand
    {
        private const string MethodName = "ExecuteCommand";

        /// <inheritdoc />
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var assembly = GetType().Assembly;

            var di = Configure(commandData, assembly);

            var commandResult = CallCommandMethod(di);

            SetMessageAndElements(ref message, elements, commandResult);
            return commandResult.Result;
        }

        private CommandDiConfigurator Configure(ExternalCommandData commandData, Assembly assembly)
        {
            var di = new CommandDiConfigurator(this, commandData);
            di.Configure(assembly);
            return di;
        }

        private CommandResult CallCommandMethod(CommandDiConfigurator di)
        {
            var methodCaller = di.Container.GetInstance<IMethodCaller<CommandResult>>();
            var commandResult = methodCaller.InvokeCommand(di.Container, MethodName);
            return commandResult;
        }

        private void SetMessageAndElements(
            ref string message,
            ElementSet elements,
            CommandResult commandResult)
        {
            if (!string.IsNullOrEmpty(commandResult.Message))
            {
                message = commandResult.Message;
            }

            if (commandResult.Elements.Any())
            {
                foreach (var element in commandResult.Elements)
                {
                    elements.Insert(element);
                }
            }
        }
    }
}