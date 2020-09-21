namespace PikTools.CommandExample.Views
{
    using System.Windows;
    using PikTools.CommandExample.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <inheritdoc/>
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            // Assign to the data context so binding can be used.
            DataContext = viewModel;
        }
    }
}
