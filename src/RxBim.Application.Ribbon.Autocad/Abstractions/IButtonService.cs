namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Autodesk.Windows;
    using Models.Configurations;
    using Button = Models.Configurations.Button;

    /// <summary>
    /// Buttons service.
    /// </summary>
    public interface IButtonService
    {
        /// <summary>
        /// Creates and returns a basic button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        /// <param name="forceTextSettings">Force settings for text placement.</param>
        /// <param name="getImage">The function to get the image of the button icon.</param>
        /// <param name="addToolTip">Add tooltip.</param>
        /// <typeparam name="T">Button type.</typeparam>
        T CreateNewButtonBase<T>(
            Button config,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings,
            Func<string?, BitmapImage?> getImage,
            bool addToolTip)
            where T : RibbonButton, new();

        /// <summary>
        /// Sets a tooltip for a button.
        /// </summary>
        /// <param name="button">Button.</param>
        /// <param name="tooltipText">Tooltip text.</param>
        /// <param name="helpUrl">Help url.</param>
        /// <param name="description">Tooltip description.</param>
        void SetTooltipForButton(RibbonItem button, string? tooltipText, string? helpUrl, string? description);

        /// <summary>
        /// Creates and returns about button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        /// <param name="getImage">The function to get the image of the button icon.</param>
        RibbonButton CreateAboutButton(
            AboutButton config,
            RibbonItemSize size,
            Orientation orientation,
            Func<string?, BitmapImage?> getImage);

        /// <summary>
        /// Clears the button cache.
        /// </summary>
        void ClearButtonCache();

        /// <summary>
        /// Applies current color theme for all buttons.
        /// </summary>
        /// <param name="getImage">The function to get the image of the button icon.</param>
        void ApplyCurrentTheme(Func<string?, BitmapImage?> getImage);
    }
}