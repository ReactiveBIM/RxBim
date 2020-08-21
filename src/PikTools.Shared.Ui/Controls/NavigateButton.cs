namespace PikTools.Shared.Ui.Controls
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Кнопка с переходом по нажатию
    /// </summary>
    public class NavigateButton : Button
    {
        /// <inheritdoc/>
        public NavigateButton()
        {
            Click += NavigateButton_Click;
        }

        /// <summary>
        /// Адрес перехода
        /// </summary>
        public Uri NavigateUri { get; set; }

        private void NavigateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            if (navigationService != null
                && NavigateUri != null)
                navigationService.Navigate(NavigateUri);
        }
    }
}
