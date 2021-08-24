namespace RxBim.Shared.Abstractions
{
    /// <summary>
    /// Отображает окно о программе
    /// </summary>
    public interface IAboutShowService
    {
        /// <summary>
        /// Показать окно о программе
        /// </summary>
        /// <param name="content">Содержимое окна</param>
        void ShowAboutBox(AboutBoxContent content);
    }
}
