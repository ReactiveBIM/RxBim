namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
    using System.Windows.Media;

    /// <summary>
    /// Defines a CAD platform-specific ribbon menu builder.
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// Constructs a CAD platform-specific ribbon.
        /// </summary>
        /// <param name="ribbonConfig">The ribbon configuration.</param>
        void BuildRibbonMenu(Ribbon? ribbonConfig = null);

        /// <summary>
        /// Returns command class type.
        /// </summary>
        /// <param name="commandTypeName">Command class type name.</param>
        Type GetCommandType(string commandTypeName);

        /// <summary>
        /// Returns tooltip content for command button.
        /// </summary>
        /// <param name="cmdButtonConfig">Command button configuration.</param>
        /// <param name="commandType">Type of command class.</param>
        string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType);

        /// <summary>
        /// Returns an image of the button's icon.
        /// </summary>
        /// <param name="resourcePath">The path to the resource.</param>
        /// <param name="assembly">The base assembly for the resource.</param>
        ImageSource? GetIconImage(string? resourcePath, Assembly? assembly);
    }
}