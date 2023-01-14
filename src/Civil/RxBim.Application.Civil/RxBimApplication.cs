namespace RxBim.Application.Civil
{
    using System;
    using Autodesk.AutoCAD.AcInfoCenterConn;
    using Autodesk.Internal.InfoCenter;
    using Shared.Civil;

    /// <inheritdoc />
    public abstract class RxBimApplication : RxBim.Application.Autocad.RxBimApplication
    {
        /// <summary>
        /// Civil is not supported.
        /// </summary>
        public event EventHandler<CivilNotSupportedEventArgs>? CivilNotSupported;

        /// <inheritdoc />
        protected override bool CanBeStarted()
        {
            if (CivilUtils.IsCivilSupported())
                return true;

            var args = new CivilNotSupportedEventArgs
                { Message = $"Application {GetType().Assembly.GetName().Name} runs only in Civil 3D." };

            CivilNotSupported?.Invoke(this, args);

            if (args.ShowMessage)
            {
                var resultItem = new ResultItem
                {
                    Title = args.Message,
                    Type = ResultType.Error
                };

                new InfoCenterManager().PaletteManager.ShowBalloon(resultItem);
            }

            return !args.StopExecution;
        }
    }
}