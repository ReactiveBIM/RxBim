namespace RxBim.Application.Menu.Fluent.Autocad.Sample.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using RxBim.Command.Autocad;
    using RxBim.Shared;

    /// <inheritdoc />
    [RxBimCommandClass("HelloCmd3Example",
        ToolTip = "Tooltip: I'm run command #3. Push me!",
        Text = "Command\n#3",
        Description = "Description: This is command #3",
        SmallImage = @"img\num3_16.png",
        LargeImage = @"img\num3_32.png",
        SmallImageLight = @"img\num3_16_light.png",
        LargeImageLight = @"img\num3_32_light.png",
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