namespace RxBim.Application.Ribbon.Abstractions
{
    using System;
    using System.Windows.Media.Imaging;
    using Models.Configurations;

    /// <summary>
    /// CAD platform-specific ribbon menu builder
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// Constructs CAD platform-specific ribbon
        /// </summary>
        /// <param name="ribbonConfig">Ribbon configuration</param>
        void BuildRibbonMenu(Ribbon? ribbonConfig = null);

        /// <summary>
        /// Returns command class type
        /// </summary>
        /// <param name="commandTypeName">Command class type name</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Type name is invalid</exception>
        Type GetCommandType(string commandTypeName);

        /// <summary>
        /// Returns tooltip content for command button
        /// </summary>
        /// <param name="cmdButtonConfig">Command button configuration</param>
        /// <param name="commandType">Type of command class</param>
        string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType);

        /// <summary>
        /// Returns an image of the button's icon
        /// </summary>
        /// <param name="fullOrRelativeImagePath">Image path</param>
        BitmapImage? GetIconImage(string? fullOrRelativeImagePath);
    }
}