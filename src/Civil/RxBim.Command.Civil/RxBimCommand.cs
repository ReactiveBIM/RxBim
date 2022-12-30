namespace RxBim.Command.Civil
{
    using Autocad.Base;
    using Autodesk.AutoCAD.ApplicationServices.Core;

    /// <summary>
    /// Civil command.
    /// </summary>
    public class RxBimCommand : RxBimCommandBase
    {
        /// <inheritdoc />
        public override void Execute()
        {
            if (!CivilUtils.IsCivilSupported())
            {
                Application.ShowAlertDialog("Команда может быть выполнена только в Civil 3D!");
                return;
            }

            base.Execute();
        }
    }
}