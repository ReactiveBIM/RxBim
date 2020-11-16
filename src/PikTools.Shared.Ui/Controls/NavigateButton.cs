namespace PikTools.Shared.Ui.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Кнопка с переходом по нажатию
    /// </summary>
    public class NavigateButton : Button
    {
        /// <summary>
        /// Свойство зависимости для элемента отображения контента
        /// </summary>
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(
                nameof(Frame),
                typeof(Frame),
                typeof(NavigateButton));

        /// <inheritdoc/>
        public NavigateButton()
        {
            Click += NavigateButton_Click;
        }

        /// <summary>
        /// Адрес перехода
        /// </summary>
        public Uri NavigateUri { get; set; }

        /// <summary>
        /// Элемент отображения контента
        /// </summary>
        public Frame Frame
        {
            get => (Frame)GetValue(FrameProperty);
            set => SetValue(FrameProperty, value);
        }

        private void NavigateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var navigationService = Frame != null
                ? Frame.NavigationService
                : NavigationService.GetNavigationService(this);
            if (navigationService != null
                && NavigateUri != null
                && (navigationService.CurrentSource == null
                    || !NavigateUri.OriginalString.Contains(navigationService.CurrentSource.OriginalString)))
                navigationService.Navigate(NavigateUri);
        }
    }
}
