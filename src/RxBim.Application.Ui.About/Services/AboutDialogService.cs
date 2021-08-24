namespace RxBim.Application.Ui.About.Services
{
    using Shared;
    using Shared.Abstractions;
    using ViewModels;
    using Views;

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
