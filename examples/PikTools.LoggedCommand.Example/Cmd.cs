namespace PikTools.LoggedCommand.Example
{
    using System;
    using System.Threading.Tasks;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
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