namespace RxBim.Application.Civil
{
    using Autocad.Base;
    using Autodesk.AutoCAD.AcInfoCenterConn;
    using Autodesk.Internal.InfoCenter;
    using Command.Civil;

    /// <inheritdoc />
    public abstract class RxBimApplication : RxBimApplicationBase
    {
        /// <inheritdoc />
        public override void Initialize()
        {
            if (!CivilUtils.IsCivilSupported())
            {
                var resultItem = new ResultItem
                {
                    Title = $"Приложение {GetType().Assembly.GetName().Name} работает, только в Civil 3D.",
                    Type = ResultType.Error
                };

                new InfoCenterManager().PaletteManager.ShowBalloon(resultItem);
                return;
            }

            base.Initialize();
        }
    }
}