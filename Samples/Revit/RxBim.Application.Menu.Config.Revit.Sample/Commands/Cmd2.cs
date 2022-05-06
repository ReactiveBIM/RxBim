namespace RxBim.Application.Menu.Config.Revit.Sample.Commands
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using RxBim.Command.Revit;
    using RxBim.Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [RxBimCommand(
        ToolTip = "Tooltip: I'm run command #2. Push me!",
        Text = "Command\n#2",
        Description = "Description: This is command #2",
        SmallImage = @"img\num2_16.png",
        LargeImage = @"img\num2_32.png",
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