namespace RxBim.Application.Civil
{
    using System;
    using Autodesk.AutoCAD.AcInfoCenterConn;
    using Autodesk.Internal.InfoCenter;
    using Command.Civil;

    /// <inheritdoc />
    public abstract class RxBimApplication : RxBim.Application.Autocad.RxBimApplication
    {
        /// <summary>
        /// Civil is not supported.
        /// </summary>
        public event EventHandler<CivilNotSupportedEventHandlerArgs>? CivilNotSupported;

        /// <inheritdoc />
        public override void Initialize()
        {
            if (CivilUtils.IsCivilSupported())
            {
                base.Initialize();
                return;
            }

            var args = new CivilNotSupportedEventHandlerArgs
            {
                Message = $"Application {GetType().Assembly.GetName().Name} runs only in Civil 3D."
            };

            CivilNotSupported?.Invoke(this, args);

            if (!args.ShowMessage)
                return;

            var resultItem = new ResultItem
            {
                Title = args.Message,
                Type = ResultType.Error
            };

            new InfoCenterManager().PaletteManager.ShowBalloon(resultItem);
        }
    }
}