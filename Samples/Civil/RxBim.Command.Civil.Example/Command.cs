namespace RxBim.Command.Civil.Example
{
    using Autocad.Base;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;

    /// <summary>
    /// Command.
    /// </summary>
    [RxBimCommandClass("RxBimExampleCivilHello")]
    public class Command : RxBimCommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello Civil!");
            return PluginResult.Succeeded;
        }
    }
}