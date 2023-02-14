namespace RxBim.Sample.Command.Civil
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using RxBim.Command.Civil;
    using Shared;
    using Shared.Autocad;

    /// <summary>
    /// Command.
    /// </summary>
    [RxBimCommandClass("RxBimExampleCivilHello")]
    public class Command : RxBimCommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Command()
        {
            CivilNotSupported += (_, args) =>
            {
                args.Message = "This is not a Civil 3D, but the command will continue.";
                args.StopExecution = false;
            };
        }

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