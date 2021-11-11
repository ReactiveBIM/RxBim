namespace RxBim.Application.Menu.Config.Revit.Sample.Commands
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using Command.Revit;
    using Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd3 : RxBimCommand
    {
        /// <summary>
        /// cmd
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            TaskDialog.Show(nameof(Cmd3), "Command executed");
            return PluginResult.Succeeded;
        }
    }
}