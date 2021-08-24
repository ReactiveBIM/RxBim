namespace RxBim.Shared.Ui.Services
{
    using System.Windows;
    using Abstractions;
    using Windows;

    /// <summary>
    /// Сервис оповещения пользователя
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUIDispatcher _uiDispatcher;
        private readonly INotificationViewModel _notificationViewModel;
        private Window _notificationWindow;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uiDispatcher">Диспетчер UI потока</param>
        /// <param name="notificationViewModel">Модель представления окна оповещения</param>
        public NotificationService(
            IUIDispatcher uiDispatcher,
            INotificationViewModel notificationViewModel)
        {
            _uiDispatcher = uiDispatcher;
            _notificationViewModel = notificationViewModel;
        }

        /// <inheritdoc />
        public void ShowMessage(string title, string text, NotificationType? type = null)
        {
            _uiDispatcher.Invoke(() =>
            {
                _notificationViewModel.SetMessage(title, text, type);
                ShowDialog();
            });
        }

        /// <inheritdoc />
        public bool ShowConfirmMessage(string title, string question)
        {
            var result = false;

            _uiDispatcher.Invoke(() =>
            {
                _notificationViewModel.SetQuestion(title, question);
                ShowDialog();
                result = _notificationViewModel.GetAnswerResult();
            });

            return result;
        }

        private void ShowDialog()
        {
            if (_notificationWindow != null
                && _notificationWindow.IsActive)
            {
                _notificationWindow.Activate();
            }
            else
            {
                _notificationWindow = new NotificationWindow
                {
                    DataContext = _notificationViewModel,
                    Topmost = true
                };
                _notificationWindow.ShowDialog();
            }
        }
    }
}
