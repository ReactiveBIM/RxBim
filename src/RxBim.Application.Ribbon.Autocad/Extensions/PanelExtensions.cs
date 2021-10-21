namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Windows;
    using Ribbon.Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Extensions for <see cref="IPanelBuilder"/>
    /// </summary>
    public static class PanelExtensions
    {
        /// <summary>
        /// Returns current row for panel
        /// </summary>
        /// <param name="panel">Panel</param>
        public static RibbonRowPanel? GetCurrentRowOrNull(this RibbonPanel panel)
        {
            return panel.Source.Items.LastOrDefault() as RibbonRowPanel;
        }

        /// <summary>
        /// Returns current row for panel
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <exception cref="InvalidOperationException">If there is no current row panel</exception>
        public static RibbonRowPanel GetCurrentRow(this RibbonPanel panel)
        {
            var currentRow = panel.GetCurrentRowOrNull();
            if (currentRow is null)
                throw new InvalidOperationException("Can't find the current panel row!");
            return currentRow;
        }

        /// <summary>
        /// Adds ribbon item to the panel
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="item">Ribbon item</param>
        public static void AddToCurrentRow(this RibbonPanel panel, RibbonItem item)
        {
            var ribbonRowPanel = panel.GetCurrentRow();
            ribbonRowPanel.Items.Add(item);
        }

        /// <summary>
        /// Panel already contains slide-out
        /// </summary>
        /// <param name="panel">Panel</param>
        public static bool HasSlideOut(this RibbonPanel panel)
        {
            return panel.Source.Items.Any(x => x is RibbonPanelBreak);
        }

        /// <summary>
        /// Creates and adds new row panel
        /// </summary>
        /// <param name="acRibbonPanel">Panel</param>
        public static void AddNewRow(this RibbonPanel acRibbonPanel)
        {
            acRibbonPanel.Source.Items.Add(new RibbonRowPanel());
        }
    }
}