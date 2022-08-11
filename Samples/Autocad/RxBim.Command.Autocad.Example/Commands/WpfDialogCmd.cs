namespace RxBim.Command.Autocad.Example.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;
    using Views;

    /// <summary>
    /// Command for an example of using a dialog window.
    /// </summary>
    public class WpfDialogCmd : RxBimCommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="window"><see cref="SomeWindow"/> instance.</param>
        public PluginResult ExecuteCommand(SomeWindow window)
        {
            Application.ShowModalWindow(window);
            return PluginResult.Succeeded;
        }
    }
}