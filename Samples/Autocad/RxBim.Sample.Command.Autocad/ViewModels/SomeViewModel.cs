namespace RxBim.Sample.Command.Autocad.ViewModels
{
    using System.Windows.Input;
    using Abstractions;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using JetBrains.Annotations;
    using Views;

    /// <summary>
    /// View model for <see cref="SomeWindow"/>.
    /// </summary>
    [UsedImplicitly]
    public class SomeViewModel : ObservableObject
    {
        private readonly ISomeService _someService;

        /// <inheritdoc/>
        public SomeViewModel(ISomeService someService)
        {
            _someService = someService;
            DoSomethingCommand = new RelayCommand(DoSomethingCommandExecute);
        }

        /// <summary>
        /// A command to do something.
        /// </summary>
        public ICommand DoSomethingCommand { get; }

        private void DoSomethingCommandExecute()
        {
            _someService.DoSomething();
        }
    }
}