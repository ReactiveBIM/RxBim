namespace PikTools.Shared.Ui.Services
{
    using PikTools.Shared;
    using PikTools.Shared.Abstractions;
    using PikTools.Shared.Ui.ViewModels;
    using PikTools.Shared.Ui.Windows;

    /// <inheritdoc/>
    public class AboutDialogService : IAboutBox
    {
        /// <inheritdoc/>
        public void ShowAboutBox(AboutBoxContent content)
        {
            var view = new AboutWindow(new AboutViewModel(content));
            view.ShowDialog();
        }
    }
}
