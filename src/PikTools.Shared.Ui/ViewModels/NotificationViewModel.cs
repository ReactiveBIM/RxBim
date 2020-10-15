namespace PikTools.Shared.Ui.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using PikTools.Shared.Ui.Abstractions;

    /// <summary>
    /// Модель представления уведомления
    /// </summary>
    public class NotificationViewModel : ViewModelBase, INotificationViewModel
    {
        private string _title;
        private NotificationType? _type;
        private string _text;
        private bool _isQuestion;
        private bool _answerResult;

        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        public string Title
        {
            get => _title;
            private set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType? Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Текст уведомления
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Является ли сообщение с вопросом к пользователю
        /// </summary>
        public bool IsQuestion
        {
            get => _isQuestion;
            set
            {
                _isQuestion = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Команда подтверждения пользователя
        /// </summary>
        public ICommand ConfirmCommand
            => new RelayCommand<IClosable>(ConfirmCommandExecute);

        /// <inheritdoc />
        public void SetMessage(string title, string text, NotificationType? type = null)
        {
            Title = title;
            Type = type;
            Text = text;
            _answerResult = false;
            IsQuestion = false;
        }

        /// <inheritdoc />
        public void SetQuestion(string title, string text)
        {
            Title = title;
            Type = null;
            Text = text;
            _answerResult = false;
            IsQuestion = true;
        }

        /// <inheritdoc />
        public bool GetAnswerResult()
        {
            return _answerResult;
        }

        private void ConfirmCommandExecute(IClosable closable)
        {
            _answerResult = true;
            closable?.Close();
        }
    }
}
