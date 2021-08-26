namespace RxBim.Application.Ribbon.Abstractions
{
    using Shared;

    /// <summary>
    /// Кнопка "О программе"
    /// </summary>
    public interface IAboutButton : IButton
    {
        /// <summary>
        /// Добавляет содержимое в окно о программе
        /// </summary>
        /// <param name="content">Содержимое окна о программе</param>
        IAboutButton SetContent(AboutBoxContent content);
    }
}