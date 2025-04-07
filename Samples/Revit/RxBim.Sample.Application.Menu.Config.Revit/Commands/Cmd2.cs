namespace RxBim.Sample.Application.Menu.Config.Revit.Commands
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using RxBim.Command.Revit;
    using Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [RxBimCommand(
        ToolTip = "Tooltip: I'm run command #2. Push me!",
        Text = "Command\n#2",
        Description = "Description: This is command #2",
        Image = @"img\num2_16.bmp",
        LargeImage = @"img\num2_32.bmp",
        HelpUrl = "https://www.google.com/")]
    public class Cmd2 : RxBimCommand
    {
        /// <summary>
        /// cmd.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            TaskDialog.Show(nameof(Cmd2), "Command executed");
            return PluginResult.Succeeded;
        }
    }
}