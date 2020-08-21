namespace PikTools.Shared.Ui.Windows
{
    using System.Windows;
    using PikTools.Shared.Ui.Abstractions;

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
