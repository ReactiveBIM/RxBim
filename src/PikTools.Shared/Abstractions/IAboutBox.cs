namespace PikTools.Shared.Abstractions
{
    /// <summary>
    /// Отображает окно о программе
    /// </summary>
    public interface IAboutBox
    {
        /// <summary>
        /// Показать окно о программе
        /// </summary>
        /// <param name="content">Содержимое окна</param>
        void ShowAboutBox(AboutBoxContent content);
    }
}
