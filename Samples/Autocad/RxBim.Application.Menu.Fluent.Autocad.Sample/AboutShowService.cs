namespace RxBim.Application.Menu.Fluent.Autocad.Sample
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Ribbon;
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