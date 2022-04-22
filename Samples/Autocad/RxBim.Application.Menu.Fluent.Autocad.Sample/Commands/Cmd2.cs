namespace RxBim.Application.Menu.Fluent.Autocad.Sample.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;

    /// <summary>
    /// Command class.
    /// </summary>
    [RxBimCommandClass("HelloCmd2Example")]
    public class Cmd2 : RxBimCommand
    {
        /// <summary>
        /// Command execution method.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello #2");
            return PluginResult.Succeeded;
        }
    }
}