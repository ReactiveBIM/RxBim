namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Windows.Media.Imaging;
    using Models;
    using Ribbon.Abstractions;

    /// <summary>
    /// Extensions for <see cref="IButton"/>
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Set the show state for the button label text
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="show">If true, show label text</param>
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
        public static IButton SetLargeImageLight(this IButton button, Uri? imageUri)
        {
            if (button is Button acButton && imageUri != null)
            {
                acButton.LargeImageLight = new BitmapImage(imageUri);
            }

            return button;
        }

        /// <summary>
        /// Sets a small image for the button in a light theme
        /// </summary>
        /// <param name="button">Button</param>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        public static IButton SetSmallImageLight(this IButton button, Uri? imageUri)
        {
            if (button is Button acButton && imageUri != null)
            {
                acButton.SmallImageLight = new BitmapImage(imageUri);
            }

            return button;
        }
    }
}