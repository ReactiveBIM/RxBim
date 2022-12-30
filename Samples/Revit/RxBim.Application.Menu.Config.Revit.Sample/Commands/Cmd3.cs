namespace RxBim.Application.Menu.Config.Revit.Sample.Commands
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using RxBim.Command.Revit;
    using Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [RxBimCommand(
        ToolTip = "Tooltip: I'm run command #3. Push me!",
        Text = "Command\n#3",
        Description = "Description: This is command #3",
        SmallImage = @"img\num3_16.jpg",
        LargeImage = @"img\num3_32.jpg",
        HelpUrl = "https://www.autodesk.com/")]
    public class Cmd3 : RxBimCommand
    {
        /// <summary>
        /// cmd.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            TaskDialog.Show(nameof(Cmd3), "Command executed");
            return PluginResult.Succeeded;
        }
    }
}