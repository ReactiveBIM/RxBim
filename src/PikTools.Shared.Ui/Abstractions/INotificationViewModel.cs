namespace PikTools.Shared.Ui.Abstractions
{
    /// <summary>
    /// Интерфейс модели представления уведомлений Revit
    /// </summary>
    public interface INotificationViewModel
    {
        /// <summary>
        /// Задать сообщение для уведомления
        /// </summary>
        /// <param name="title">Заголовок сообщения</param>
        /// <param name="text">Текст сообщения</param>
        void SetMessage(string title, string text);

        /// <summary>
        /// Задать сообщение с вопросом
        /// </summary>
        /// <param name="title">Заголовок сообщения</param>
        /// <param name="text">Текст сообщения</param>
        void SetQuestion(string title, string text);

        /// <summary>
        /// Получить результат по вопросу к пользователю
        /// </summary>
        bool GetAnswerResult();
    }
}
