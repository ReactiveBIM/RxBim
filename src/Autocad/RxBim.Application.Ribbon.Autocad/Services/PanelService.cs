namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Autodesk.Windows;

    /// <inheritdoc />
    internal class PanelService : IPanelService
    {
        private readonly IRibbonComponentStorageService _storageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelService"/> class.
        /// </summary>
        /// <param name="storageService"><see cref="IRibbonComponentStorageService"/>.</param>
        public PanelService(IRibbonComponentStorageService storageService)
        {
            _storageService = storageService;
        }

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

            var ribbonPanelBreak = new RibbonPanelBreak();
            panel.Source.Items.Add(ribbonPanelBreak);
            _storageService.AddItem(ribbonPanelBreak, panel.Source.Items);
            AddNewRow(panel);
        }

        /// <inheritdoc />
        public void AddItem(RibbonPanel panel, RibbonItem item)
        {
            var ribbonRowPanel = GetCurrentRow(panel);
            ribbonRowPanel.Items.Add(item);
            _storageService.AddItem(item, ribbonRowPanel.Items);
        }

        /// <inheritdoc />
        public RibbonPanel GetOrCreatePanel(RibbonTab tab, string panelName)
        {
            var panel = tab.Panels.FirstOrDefault(x =>
                x.Source.Name != null &&
                x.Source.Name.Equals(panelName, StringComparison.OrdinalIgnoreCase));
            if (panel is null)
            {
                panel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Name = panelName,
                        Title = panelName,
                        Id = $"{tab.Id}_PANEL_{panelName.GetHashCode():0}"
                    },
                };

                tab.Panels.Add(panel);
                _storageService.AddPanel(panel);
            }

            if (GetCurrentRowOrNull(panel) is null)
                AddNewRow(panel);

            return panel;
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
            var ribbonRowPanel = new RibbonRowPanel();
            acRibbonPanel.Source.Items.Add(ribbonRowPanel);
            _storageService.AddItem(ribbonRowPanel, acRibbonPanel.Source.Items);
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