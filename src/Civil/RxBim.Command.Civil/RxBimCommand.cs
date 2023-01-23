namespace RxBim.Command.Civil
{
    using System;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared.Civil;

    /// <summary>
    /// Civil command.
    /// </summary>
    public class RxBimCommand : Autocad.RxBimCommand
    {
        /// <summary>
        /// Civil is not supported.
        /// </summary>
        public event EventHandler<CivilNotSupportedEventArgs>? CivilNotSupported;

        /// <inheritdoc />
        public override void Execute()
        {
            if (!CivilUtils.IsCivilSupported())
            {
                var args = new CivilNotSupportedEventArgs
                    { Message = "The command can only be executed in Civil 3D!" };

                CivilNotSupported?.Invoke(this, args);

                if (args.ShowMessage)
                    Application.ShowAlertDialog(args.Message);

                if (args.StopExecution)
                    return;
            }

            base.Execute();
        }
    }
}