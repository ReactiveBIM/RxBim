namespace RxBim.Sample.Application.Menu.Config.Autocad.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;
    using Shared.Autocad;

    /// <inheritdoc />
    [RxBimCommandClass("HelloCmd3Example",
        ToolTip = "Tooltip: I'm run command #3. Push me!",
        Text = "Command\n#3",
        Description = "Description: This is command #3",
        Image = @"img\num3_16.jpg",
        LargeImage = @"img\num3_32.jpg",
        SmallImageLight = @"img\num3_16_light.jpg",
        LargeImageLight = @"img\num3_32_light.jpg",
        HelpUrl = "https://www.autodesk.com/")]
    public class Cmd3 : RxBimCommand
    {
        /// <summary>
        /// Command execution.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello #3");
            return PluginResult.Succeeded;
        }
    }
}