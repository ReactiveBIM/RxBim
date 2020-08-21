namespace PikTools.CommandExample.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using PikTools.CommandExample.Abstractions;
    using PikTools.Shared.Ui.Abstractions;
    using PikTools.Shared.Ui.ViewModels;

    /// <summary>
    /// Основная модель представления
    /// </summary>
    public class MainWindowViewModel : MainViewModelBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMyService _myService;

        private int _intValue;

        /// <inheritdoc/>
        public MainWindowViewModel(
            INotificationService notificationService,
            IMyService myService)
            : base("Тестовый плагин")
        {
            _notificationService = notificationService;
            _myService = myService;

            InitializeCommand = new RelayCommand(InitializeCommandExecute);
        }

        /// <summary>
        /// Числовое значение от пользователя
        /// </summary>
        public int IntValue
        {
            get => _intValue;
            set
            {
                _intValue = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Команда выполнения
        /// </summary>
        public ICommand DoCommand => new RelayCommand(DoCommandExecute);

        private void InitializeCommandExecute()
        {
            // Initialize
        }

        private void DoCommandExecute()
        {
            try
            {
                if (IntValue < 0
                    || IntValue > 10)
                {
                    _notificationService.ShowMessage("Внимание!", "Введите число от 0 до 10!");
                    return;
                }

                _myService.Go();
            }
            catch (Exception exception)
            {
                _notificationService.ShowMessage("Внимание!", exception.ToString());
            }
        }
    }
}
