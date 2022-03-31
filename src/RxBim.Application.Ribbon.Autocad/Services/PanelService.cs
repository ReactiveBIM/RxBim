namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Windows;

    /// <inheritdoc />
    public class PanelService : IPanelService
    {
        /// <inheritdoc />
        public void AddSeparator(RibbonPanel panel)
        {
            AddItem(panel, new RibbonSeparator());
        }

        /// <inheritdoc />
        public void AddSlideOut(RibbonPanel panel)
        {
            if (HasSlideOut(panel))
                return;

            panel.Source.Items.Add(new RibbonPanelBreak());
            AddNewRow(panel);
        }

        /// <inheritdoc />
        public void AddItem(RibbonPanel panel, RibbonItem item)
        {
            var ribbonRowPanel = GetCurrentRow(panel);
            ribbonRowPanel.Items.Add(item);
        }

        /// <inheritdoc />
        public RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName)
        {
            var acRibbonPanel = acRibbonTab.Panels.FirstOrDefault(x =>
                x.Source.Name != null &&
                x.Source.Name.Equals(panelName, StringComparison.OrdinalIgnoreCase));
            if (acRibbonPanel is null)
            {
                acRibbonPanel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Name = panelName,
                        Title = panelName,
                        Id = $"{acRibbonTab.Id}_PANEL_{panelName.GetHashCode():0}"
                    },
                };

                acRibbonTab.Panels.Add(acRibbonPanel);
            }

            if (GetCurrentRowOrNull(acRibbonPanel) is null)
                AddNewRow(acRibbonPanel);

            return acRibbonPanel;
        }

        /// <inheritdoc />
        public void DeletePanel(RibbonPanel panel)
        {
            panel.Tab.Panels.Remove(panel);
        }

        /// <summary>
        /// Panel already contains slide-out
        /// </summary>
        /// <param name="panel">Panel</param>
        private bool HasSlideOut(RibbonPanel panel)
        {
            return panel.Source.Items.Any(x => x is RibbonPanelBreak);
        }

        /// <summary>
        /// Creates and adds new row panel
        /// </summary>
        /// <param name="acRibbonPanel">Panel</param>
        private void AddNewRow(RibbonPanel acRibbonPanel)
        {
            acRibbonPanel.Source.Items.Add(new RibbonRowPanel());
        }

        /// <summary>
        /// Returns current row for panel
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <exception cref="InvalidOperationException">If there is no current row panel</exception>
        private RibbonRowPanel GetCurrentRow(RibbonPanel panel)
        {
            var currentRow = GetCurrentRowOrNull(panel);
            if (currentRow is null)
                throw new InvalidOperationException("Can't find the current panel row!");
            return currentRow;
        }

        /// <summary>
        /// Returns current row for panel
        /// </summary>
        /// <param name="panel">Panel</param>
        private RibbonRowPanel? GetCurrentRowOrNull(RibbonPanel panel)
        {
            return panel.Source.Items.LastOrDefault() as RibbonRowPanel;
        }
    }
}