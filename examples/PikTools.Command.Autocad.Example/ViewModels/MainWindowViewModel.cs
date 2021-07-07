namespace PikTools.Command.Autocad.Example.ViewModels
{
    using System.Windows.Input;
    using Abstractions;
    using GalaSoft.MvvmLight.Command;
    using Shared.Ui.Abstractions;
    using Shared.Ui.ViewModels;

    /// <summary>
    /// Основная модель представления
    /// </summary>
    public class MainWindowViewModel : MainViewModelBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMyService _myService;

        /// <inheritdoc/>
        public MainWindowViewModel(
            INotificationService notificationService,
            IMyService myService)
            : base("Тестовый плагин")
        {
            _notificationService = notificationService;
            _myService = myService;
        }

        /// <summary>
        /// Команда выполнения
        /// </summary>
        public ICommand DoCommand => new RelayCommand(DoCommandExecute);

        private void DoCommandExecute()
        {
            _myService.Go();
            _notificationService.ShowMessage("Заголовок", "Готово");
        }
    }
}
