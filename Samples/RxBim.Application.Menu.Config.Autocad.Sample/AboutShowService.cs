namespace RxBim.Application.Menu.Config.Autocad.Sample
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;
    using Shared.Abstractions;

    /// <inheritdoc />
    public class AboutShowService : IAboutShowService
    {
        /// <inheritdoc />
        public void ShowAboutBox(AboutBoxContent content)
        {
            Application.ShowAlertDialog(content.ToString());
        }
    }
}