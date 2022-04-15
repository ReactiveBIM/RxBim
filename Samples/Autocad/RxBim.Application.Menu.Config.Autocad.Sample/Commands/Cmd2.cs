namespace RxBim.Application.Menu.Config.Autocad.Sample.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;

    /// <inheritdoc />
    [RxBimCommandClass("HelloCmd2Example")]
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