namespace RxBim.Command.Civil.Example
{
    using Autocad;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;

    /// <summary>
    /// Command.
    /// </summary>
    [RxBimCommandClass("RxBimExampleCivilHello")]
    public class Command : RxBim.Command.Civil.RxBimCommand
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Command()
        {
            CivilNotSupported += (_, args) =>
            {
                args.Message = "Your message";
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