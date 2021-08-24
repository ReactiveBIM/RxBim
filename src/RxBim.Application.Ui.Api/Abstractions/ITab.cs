namespace RxBim.Application.Ui.Api.Abstractions
{
    using System;

    /// <summary>
    /// Вкладка
    /// </summary>
    public interface ITab : IRibbonBuilder
    {
        /// <summary>
        /// Создает панель на закладке
        /// </summary>
        /// <param name="panelTitle">имя панели</param>
        IPanel Panel(string panelTitle);

        /// <summary>
        /// Создает кнопку о программе
        /// </summary>
        /// <param name="name">Название кнопки</param>
        /// <param name="action">Дополнительны действия для кнопки о программе</param>
        /// <param name="panelName">имя панели</param>
        /// <param name="text">Тест описания</param>
        ITab AboutButton(
            string name,
            Action<IAboutButton> action,
            string panelName = null,
            string text = null);
    }
}