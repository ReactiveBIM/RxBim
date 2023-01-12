namespace RxBim.Command.Civil
{
    using System;
    using Application.Civil;
    using Autodesk.AutoCAD.ApplicationServices.Core;

    /// <summary>
    /// Civil command.
    /// </summary>
    public class RxBimCommand : RxBim.Command.Autocad.RxBimCommand
    {
        /// <summary>
        /// Civil is not supported.
        /// </summary>
        public event EventHandler<CivilNotSupportedEventHandlerArgs>? CivilNotSupported;

        /// <inheritdoc />
        public override void Execute()
        {
            if (CivilUtils.IsCivilSupported())
            {
                base.Execute();
                return;
            }

            var args = new CivilNotSupportedEventHandlerArgs
            {
                Message = "The command can only be executed in Civil 3D!"
            };

            CivilNotSupported?.Invoke(this, args);

            if (args.ShowMessage)
                Application.ShowAlertDialog(args.Message);
        }
    }
}