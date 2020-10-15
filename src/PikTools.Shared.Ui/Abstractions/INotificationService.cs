namespace PikTools.Shared.Ui.Abstractions
{
    /// <summary>
    /// Интерфейс сервиса уведомлений
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Показать сообщение пользователю
        /// </summary>
        /// <param name="title">Заголовок сообщения</param>
        /// <param name="text">Текст сообщения</param>
        /// <param name="type">Тип уведомления</param>
        void ShowMessage(string title, string text, NotificationType? type = null);

        /// <summary>
        /// Показать сообщение подтверждения от пользователя
        /// </summary>
        /// <param name="title">Заголовок сообщения</param>
        /// <param name="question">Вопрос для подтверждения</param>
        /// <returns>true - пользователь подтвердил, иначе - false</returns>
        bool ShowConfirmMessage(string title, string question);
    }
}
