namespace PikTools.CommandExample
{
    using Autodesk.Revit.Attributes;
    using Command.Api;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd : PikToolsCommand
    {
        /// <summary>
        /// cmd
        /// </summary>
        /// <param name="service">service</param>
        public CommandResult ExecuteCommand(IMyService service)
        {
            service.Go();
            return CommandResult.Succeeded;
        }
    }
}