namespace RxBim.Sample.Command.Autocad.Views
{
    using ViewModels;

    /// <content>
    /// Interaction logic for SomeWindow.xaml
    /// </content>
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