namespace RxBim.Application.Menu.Fluent.Autocad.Sample.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Command.Autocad;
    using Shared;

    /// <inheritdoc />
    [RxBimCommandClass("HelloCmd1Example")]
    public class Cmd1 : RxBimCommand
    {
        /// <summary>
        /// Command execution method
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello #1");
            return PluginResult.Succeeded;
        }
    }
}