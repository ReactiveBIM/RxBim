namespace RxBim.Shared.Ui.Windows
{
    using System.Windows;
    using Abstractions;

    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window, IClosable
    {
        /// <inheritdoc/>
        public NotificationWindow()
        {
            InitializeComponent();
        }
    }
}
