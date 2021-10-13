namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Windows.Media.Imaging;
    using Models;
    using Ribbon.Abstractions;

    /// <summary>
    /// Extensions for <see cref="IButtonBuilder"/>
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Set the show state for the button label text
        /// </summary>
        /// <param name="buttonBuilder">Button</param>
        /// <param name="show">If true, show label text</param>
        public static IButtonBuilder SetShowText(this IButtonBuilder buttonBuilder, bool show)
        {
            if (buttonBuilder is ButtonBuilder acadButton)
            {
                acadButton.ShowText = show;
            }

            return buttonBuilder;
        }

        /// <summary>
        /// Sets a large image for the button in a light theme
        /// </summary>
        /// <param name="buttonBuilder">Button</param>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        public static IButtonBuilder SetLargeImageLight(this IButtonBuilder buttonBuilder, Uri? imageUri)
        {
            if (buttonBuilder is ButtonBuilder acButton && imageUri != null)
            {
                acButton.LargeImageLight = new BitmapImage(imageUri);
            }

            return buttonBuilder;
        }

        /// <summary>
        /// Sets a small image for the button in a light theme
        /// </summary>
        /// <param name="buttonBuilder">Button</param>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        public static IButtonBuilder SetSmallImageLight(this IButtonBuilder buttonBuilder, Uri? imageUri)
        {
            if (buttonBuilder is ButtonBuilder acButton && imageUri != null)
            {
                acButton.SmallImageLight = new BitmapImage(imageUri);
            }

            return buttonBuilder;
        }
    }
}