namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using Models;
    using Ribbon.Abstractions;

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

        /// <summary>
        /// Sets a large image for the button in a light theme
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        public static IButton SetLargeImageLight(this IButton button, Uri imageUri)
        {
        }

        /// <summary>
        /// Sets a small image for the button in a light theme
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        public static IButton SetSmallImageLight(this IButton button, Uri imageUri)
        {
        }
    }
}