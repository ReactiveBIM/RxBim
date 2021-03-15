namespace PikTools.Shared.Ui.Windows
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Navigation;
    using PikTools.Shared.Ui.ViewModels;

    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <inheritdoc/>
        public AboutWindow(AboutViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
