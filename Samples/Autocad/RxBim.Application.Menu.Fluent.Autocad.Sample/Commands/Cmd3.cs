namespace RxBim.Application.Menu.Fluent.Autocad.Sample.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;

    /// <summary>
    /// Command class.
    /// </summary>
    [RxBimCommandClass("HelloCmd3Example")]
    public class Cmd3 : RxBimCommand
    {
        /// <summary>
        /// Command execution method.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello #3");
            return PluginResult.Succeeded;
        }
    }
}