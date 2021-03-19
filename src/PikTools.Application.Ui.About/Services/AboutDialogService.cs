namespace PikTools.Application.Ui.About.Services
{
    using PikTools.Application.Ui.About.ViewModels;
    using PikTools.Application.Ui.About.Views;
    using PikTools.Shared;
    using PikTools.Shared.Abstractions;

    /// <inheritdoc/>
    public class AboutDialogService : IAboutShowService
    {
        /// <inheritdoc/>
        public void ShowAboutBox(AboutBoxContent content)
        {
            var view = new AboutWindow(new AboutViewModel(content));
            view.ShowDialog();
        }
    }
}
