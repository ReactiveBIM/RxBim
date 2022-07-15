namespace RxBim.Application.Autocad.Example.Services
{
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using JetBrains.Annotations;

    /// <inheritdoc />
    [UsedImplicitly]
    public class InfoService : IInfoService
    {
        /// <inheritdoc/>
        public void ShowAutocadVersion()
        {
            Application.ShowAlertDialog($"AutoCAD version: {Application.Version}");
        }
    }
}