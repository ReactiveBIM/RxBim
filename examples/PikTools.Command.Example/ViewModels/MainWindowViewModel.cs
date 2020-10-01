namespace PikTools.CommandExample.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using PikTools.CommandExample.Abstractions;
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.Ui.Abstractions;
    using PikTools.Shared.Ui.Commands;
    using PikTools.Shared.Ui.ViewModels;

    /// <summary>
    /// Основная модель представления
    /// </summary>
    public class MainWindowViewModel : MainViewModelBase
    {
        private readonly INotificationService _notificationService;
        private readonly IScopedElementsCollector _scopedElementsCollector;
        private readonly IMyService _myService;

        private ScopeType _scope = ScopeType.ActiveView;
        private int _intValue;
        private List<string> _selectedChoises
            = new List<string>();

        /// <inheritdoc/>
        public MainWindowViewModel(
            INotificationService notificationService,
            IScopedElementsCollector scopedElementsCollector,
            IMyService myService)
            : base("Тестовый плагин")
        {
            _notificationService = notificationService;
            _scopedElementsCollector = scopedElementsCollector;
            _myService = myService;

            InitializeCommand = new RelayCommand(InitializeCommandExecute);
        }

        /// <summary>
        /// Выбранный тип обработки
        /// </summary>
        public ScopeType Scope
        {
            get => _scope;
            set
            {
                _scope = value;
                RaisePropertyChanged();
            }
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
        /// Индексы для доступного выбора
        /// </summary>
        public ObservableCollection<int> Indexes { get; }
            = new ObservableCollection<int>();

        /// <summary>
        /// Доступный выбор
        /// </summary>
        public ObservableCollection<string> AvailableChoises { get; }
            = new ObservableCollection<string>();

        /// <summary>
        /// Выбранные значения
        /// </summary>
        public List<string> SelectedChoises
        {
            get => _selectedChoises;
            set
            {
                _selectedChoises = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Команда выполнения
        /// </summary>
        public ICommand DoCommand => new RelayAsyncCommand(DoCommandExecute);

        private void InitializeCommandExecute()
        {
            for (var i = 1; i <= 5; i++)
            {
                Indexes.Add(i);
                AvailableChoises.Add($"Choise {i}");
            }
        }

        private async Task DoCommandExecute()
        {
            try
            {
                _scopedElementsCollector.SetScope(Scope);

                if (IntValue < 0
                    || IntValue > 10)
                {
                    _notificationService.ShowMessage("Внимание!", "Введите число от 0 до 10!");
                    return;
                }

                var goResult = await _myService.Go();
                if (goResult.IsFailure)
                    _notificationService.ShowMessage("Внимание!", goResult.Error);
            }
            catch (Exception exception)
            {
                _notificationService.ShowMessage("Внимание!", exception.ToString());
            }
            finally
            {
                _scopedElementsCollector.SetBackSelectedElements();
            }
        }
    }
}
