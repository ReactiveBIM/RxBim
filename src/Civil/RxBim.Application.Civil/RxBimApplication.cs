namespace RxBim.Application.Civil
{
    using System;
    using Autocad.Base;
    using Autodesk.AutoCAD.AcInfoCenterConn;
    using Autodesk.Internal.InfoCenter;
    using Command.Civil;

    public delegate void CivilNotSupportedEventHandler(object sender, CivilNotSupportedEventHandlerArgs args);

    /// <inheritdoc />
    public abstract class RxBimApplication : RxBimApplicationBase
    {
        /// <summary>
        /// Civil is not supported.
        /// </summary>
        public event CivilNotSupportedEventHandler? NotSupportedDetected;

        /// <inheritdoc />
        public override void Initialize()
        {
            if (!CivilUtils.IsCivilSupported())
            {
                var args = new CivilNotSupportedEventHandlerArgs
                {
                    Message = $"Application {GetType().Assembly.GetName().Name} runs only in Civil 3D."
                };

                NotSupportedDetected?.Invoke(this, args);

                if (!args.ShowMessage)
                    return;

                var resultItem = new ResultItem
                {
                    Title = args.Message,
                    Type = ResultType.Error
                };

                new InfoCenterManager().PaletteManager.ShowBalloon(resultItem);

                return;
            }

            base.Initialize();
        }
    }
}