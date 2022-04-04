namespace RxBim.Shared.Abstractions
{
    using System;

    /// <summary>
    /// Отображает окно о программе
    /// </summary>
    [Obsolete("Will be deleted at the next release!")]
    public interface IAboutShowService
    {
        /// <summary>
        /// Показать окно о программе
        /// </summary>
        /// <param name="content">Содержимое окна</param>
        void ShowAboutBox(AboutBoxContent content);
    }
}