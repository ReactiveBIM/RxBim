namespace RxBim.Application.Ui.Autocad.Api.Extensions
{
    using Models;
    using RxBim.Application.Ui.Api.Abstractions;

    /// <summary>
    /// Расширения для кнопки
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Задать свойство отображения текста
        /// </summary>
        /// <param name="button">Кнопка</param>
        /// <param name="show">Отображать текст</param>
        public static IButton SetShowText(this IButton button, bool show)
        {
            if (button is Button acadButton)
            {
                acadButton.ShowText = show;
            }

            return button;
        }
    }
}