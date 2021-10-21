namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Windows.Controls;
    using Autodesk.Windows;
    using Models.Configurations;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Button = Models.Configurations.Button;

    /// <summary>
    /// Extensions for <see cref="IButtonBuilder"/>
    /// </summary>
    internal static class ButtonExtensions
    {
        /// <summary>
        /// Sets tooltip for command button
        /// </summary>
        /// <param name="ribbonButton">Ribbon button</param>
        /// <param name="cmdButtonConfig">Command button config</param>
        public static void SetTooltipForCommandButton(this RibbonButton ribbonButton, CommandButton cmdButtonConfig)
        {
            var hasToolTip = !string.IsNullOrWhiteSpace(cmdButtonConfig.ToolTip);
            var hasHelpUrl = !string.IsNullOrWhiteSpace(cmdButtonConfig.HelpUrl);

            if (!hasToolTip && !hasHelpUrl)
                return;

            var toolTip = new RibbonToolTip();

            if (hasToolTip)
            {
                toolTip.Content = cmdButtonConfig.ToolTip;
            }

            if (hasHelpUrl)
            {
                toolTip.HelpTopic = cmdButtonConfig.HelpUrl;
                toolTip.IsHelpEnabled = true;
            }
            else
            {
                toolTip.IsHelpEnabled = false;
            }

            ribbonButton.ToolTip = toolTip;
        }

        /// <summary>
        /// Sets tooltip for non-command button
        /// </summary>
        /// <param name="ribbonButton">Ribbon button</param>
        /// <param name="buttonConfig">Button config</param>
        public static void SetTooltipForNonCommandButton(this RibbonButton ribbonButton, Button buttonConfig)
        {
            if (string.IsNullOrWhiteSpace(buttonConfig.ToolTip))
                return;

            var toolTip = new RibbonToolTip
            {
                Content = buttonConfig.ToolTip,
                IsHelpEnabled = false
            };
            ribbonButton.ToolTip = toolTip;
        }

        /// <summary>
        /// Sets the base properties for the ribbon button from the button configuration
        /// </summary>
        /// <param name="ribbonButton">Ribbon button</param>
        /// <param name="buttonConfig">Button configuration</param>
        /// <param name="isSmall">Button is small</param>
        /// <exception cref="InvalidOperationException">If the button name is not specified</exception>
        public static void SetButtonProperties(this RibbonButton ribbonButton, Button buttonConfig, bool isSmall)
        {
            if (string.IsNullOrWhiteSpace(buttonConfig.Name))
                throw new InvalidOperationException("Button name can't be null or empty!");
            ribbonButton.Name = buttonConfig.Name;

            if (!string.IsNullOrWhiteSpace(buttonConfig.Text))
            {
                ribbonButton.Text = buttonConfig.Text;
                ribbonButton.ShowText = true;
            }

            if (!string.IsNullOrWhiteSpace(buttonConfig.Description))
            {
                ribbonButton.Description = buttonConfig.Description;
            }

            ribbonButton.IsCheckable = false;
            ribbonButton.ShowImage = true;

            if (isSmall)
            {
                ribbonButton.Orientation = Orientation.Horizontal;
                ribbonButton.Size = RibbonItemSize.Standard;
            }
            else
            {
                ribbonButton.Orientation = Orientation.Vertical;
                ribbonButton.Size = RibbonItemSize.Large;
            }
        }
    }
}