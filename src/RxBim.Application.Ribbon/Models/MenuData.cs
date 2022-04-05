namespace RxBim.Application.Ribbon.Models
{
    using System;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Configurations;
    using Extensions;

    /// <summary>
    /// Menu data.
    /// </summary>
    public class MenuData
    {
        /// <summary>
        /// Ribbon configuration
        /// </summary>
        public Ribbon? RibbonConfiguration { get; set; }

        /// <summary>
        /// Menu defining assembly.
        /// </summary>
        public Assembly? MenuAssembly { get; set; }

        /// <summary>
        /// Returns an image of the button's icon
        /// </summary>
        /// <param name="fullOrRelativeImagePath">Image path</param>
        public BitmapImage? GetIconImage(string? fullOrRelativeImagePath)
        {
            if (MenuAssembly is null || string.IsNullOrWhiteSpace(fullOrRelativeImagePath))
                return null;
            var uri = MenuAssembly.TryGetSupportFileUri(fullOrRelativeImagePath!);
            return uri != null ? new BitmapImage(uri) : null;
        }

        /// <summary>
        /// Returns tooltip content for command button
        /// </summary>
        /// <param name="cmdButtonConfig">Command button configuration</param>
        /// <param name="commandType">Type of command class</param>
        public string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType)
        {
            var toolTip = cmdButtonConfig.ToolTip;
            if (RibbonConfiguration is null || toolTip is null || !RibbonConfiguration.AddVersionToCommandTooltip)
                return toolTip;
            if (toolTip.Length > 0)
                toolTip += Environment.NewLine;
            toolTip += $"{RibbonConfiguration.CommandTooltipVersionHeader}{commandType.Assembly.GetName().Version}";
            return toolTip;
        }
    }
}