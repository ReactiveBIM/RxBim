namespace RxBim.Application.Ribbon
{
    using System.Windows.Controls;
    using Autodesk.Windows;

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
        /// <param name="addToolTip">Add tooltip.</param>
        /// <typeparam name="T">Button type.</typeparam>
        T CreateNewButton<T>(
            Button config,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings,
            bool addToolTip)
            where T : RibbonButton, new();

        /// <summary>
        /// Creates and returns a command button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        RibbonButton CreateCommandButton(CommandButton config, RibbonItemSize size, Orientation? orientation = null);

        /// <summary>
        /// Creates and returns a pull-down button.
        /// </summary>
        /// <param name="config">Button configuration.</param>
        /// <param name="size">Button size.</param>
        /// <param name="orientation">Button orientation.</param>
        RibbonSplitButton CreatePullDownButton(
            PullDownButton config,
            RibbonItemSize size,
            Orientation orientation);

        /// <summary>
        /// Clears the button cache.
        /// </summary>
        void ClearButtonCache();

        /// <summary>
        /// Applies current color theme for all buttons.
        /// </summary>
        void ApplyCurrentTheme();
    }
}