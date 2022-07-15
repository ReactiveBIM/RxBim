namespace RxBim.Command.Autocad.Example.Views
{
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SomeWindow
    {
        /// <inheritdoc/>
        public SomeWindow(SomeViewModel viewModel)
        {
            InitializeComponent();

            // Assign to the data context so binding can be used.
            DataContext = viewModel;
        }
    }
}