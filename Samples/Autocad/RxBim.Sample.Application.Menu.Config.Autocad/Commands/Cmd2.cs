namespace RxBim.Sample.Application.Menu.Config.Autocad.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;
    using Shared.Autocad;

    /// <inheritdoc />
    [RxBimCommandClass("HelloCmd2Example",
        ToolTip = "Tooltip: I'm run command #2. Push me!",
        Text = "Command\n#2",
        Description = "Description: This is command #2",
        SmallImage = @"img\num2_16.bmp",
        LargeImage = @"img\num2_32.bmp",
        SmallImageLight = @"img\num2_16_light.bmp",
        LargeImageLight = @"img\num2_32_light.bmp",
        HelpUrl = "https://www.google.com/")]
    public class Cmd2 : RxBimCommand
    {
        /// <summary>
        /// Command execution.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello #2");
            return PluginResult.Succeeded;
        }
    }
}