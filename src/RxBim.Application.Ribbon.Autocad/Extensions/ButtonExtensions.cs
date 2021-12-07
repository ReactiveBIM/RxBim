namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Windows.Controls;
    using Autodesk.Windows;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Button = Models.Configurations.Button;

    /// <summary>
    /// Extensions for <see cref="IButtonBuilder"/>
    /// </summary>
    internal static class ButtonExtensions
    {
        /// <summary>
        /// Sets tooltip for button
        /// </summary>
        /// <param name="ribbonButton">Ribbon button</param>
        /// <param name="tooltipText">Tooltip text</param>
        /// <param name="helpUrl">Help url string</param>
        /// <param name="description">Description</param>
        /// <param name="newToolTipAction">Action for a new tooltip.</param>
        public static void SetTooltipForButton(
            this RibbonButton ribbonButton,
            string? tooltipText,
            string? helpUrl,
            string? description,
            Action<RibbonToolTip> newToolTipAction)
        {
            var hasToolTip = !string.IsNullOrWhiteSpace(tooltipText);
            var hasHelpUrl = !string.IsNullOrWhiteSpace(helpUrl);

            if (!hasToolTip && !hasHelpUrl)
                return;

            var toolTip = new RibbonToolTip
                { Title = string.IsNullOrWhiteSpace(ribbonButton.Text) ? ribbonButton.Name : ribbonButton.Text };

            if (!string.IsNullOrWhiteSpace(description))
                toolTip.ExpandedContent = description;

            if (hasToolTip)
                toolTip.Content = tooltipText;

            if (hasHelpUrl)
            {
                toolTip.HelpTopic = helpUrl;
                toolTip.IsHelpEnabled = true;
            }
            else
            {
                toolTip.IsHelpEnabled = false;
            }

            newToolTipAction.Invoke(toolTip);

            ribbonButton.ToolTip = toolTip;
        }

        /// <summary>
        /// Sets the base properties for the ribbon button from the button configuration
        /// </summary>
        /// <param name="ribbonButton">Ribbon button</param>
        /// <param name="buttonConfig">Button configuration</param>
        /// <param name="size">Button size</param>
        /// <param name="orientation">Button orientation</param>
        /// <param name="forceTextSettings">Force settings for text placement</param>
        /// <exception cref="InvalidOperationException">If the button name is not specified</exception>
        public static void SetButtonProperties(
            this RibbonButton ribbonButton,
            Button buttonConfig,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings)
        {
            if (string.IsNullOrWhiteSpace(buttonConfig.Name))
                throw new InvalidOperationException("Button name can't be null or empty!");

            ribbonButton.Name = buttonConfig.Name;
            ribbonButton.Size = size;
            ribbonButton.Orientation = orientation;

            var hasText = !string.IsNullOrWhiteSpace(buttonConfig.Text);
            if (hasText)
            {
                ribbonButton.Text = buttonConfig.Text;
            }

            if (hasText || forceTextSettings)
            {
                ribbonButton.ShowText = true;
            }

            if (!string.IsNullOrWhiteSpace(buttonConfig.Description))
            {
                ribbonButton.Description = buttonConfig.Description;
            }

            ribbonButton.IsCheckable = false;
            ribbonButton.ShowImage = true;
        }

        /// <summary>
        /// Returns orientation for single large button
        /// </summary>
        /// <param name="button">Button config</param>
        public static Orientation GetSingleLargeButtonOrientation(this Button button)
        {
            return !string.IsNullOrWhiteSpace(button.Text)
                ? Orientation.Vertical
                : Orientation.Horizontal;
        }
    }
}