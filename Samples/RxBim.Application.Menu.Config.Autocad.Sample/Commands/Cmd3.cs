namespace RxBim.Application.Menu.Config.Autocad.Sample.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;

    /// <inheritdoc />
    [RxBimCommandClass("HelloCmd3Example")]
    public class Cmd3 : RxBimCommand
    {
        /// <summary>
        /// Command execution
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello #3");
            return PluginResult.Succeeded;
        }
    }
}