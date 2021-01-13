namespace PikTools.LoggedCommand.Example
{
    using Autodesk.Revit.Attributes;
    using Command.Api;
    using Shared;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd : PikToolsCommand
    {
        public PluginResult ExecuteCommand()
        {
            return PluginResult.Succeeded;
        }
    }
}