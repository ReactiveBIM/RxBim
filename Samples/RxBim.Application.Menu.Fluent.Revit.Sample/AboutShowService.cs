namespace RxBim.Application.Menu.Fluent.Revit.Sample
{
    using Autodesk.Revit.UI;
    using Shared;
    using Shared.Abstractions;

    /// <inheritdoc />
    public class AboutShowService : IAboutShowService
    {
        /// <inheritdoc />
        public void ShowAboutBox(AboutBoxContent content)
        {
            TaskDialog.Show(content.Title, content.ToString());
        }
    }
}