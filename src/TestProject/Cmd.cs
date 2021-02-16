namespace TestProject
{
    using Autodesk.Revit.Attributes;
    using PikTools.Command.Api;
    using PikTools.Shared;

    /// <inheritdoc />
    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class Cmd : PikToolsCommand
    {
        /// <inheritdoc />
        public PluginResult ExecuteCommand()
        {
            return PluginResult.Succeeded;
        }
    }
}