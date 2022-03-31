namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using System.Windows.Controls;
    using Autodesk.Windows;
    using Ribbon.Models.Configurations;

    /// <summary>
    /// Buttons service.
    /// </summary>
    public interface IButtonService
    {
        /// <summary>
        /// Creates and returns about button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        RibbonButton CreateAboutButton(AboutButton config, RibbonItemSize size, Orientation orientation);

        /// <summary>
        /// Creates and returns command button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        /// <returns></returns>
        RibbonButton CreateCommandButton(CommandButton config, RibbonItemSize size, Orientation orientation);

        /// <summary>
        /// Creates and returns pull-down button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        /// <returns></returns>
        RibbonSplitButton CreatePullDownButton(PullDownButton config, RibbonItemSize size, Orientation orientation);

        /// <summary>
        /// Clears the button cache.
        /// </summary>
        void ClearButtonCache();

        /// <summary>
        /// Applies the color theme to all buttons.
        /// </summary>
        void ApplyCurrentTheme();
    }
}