namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Windows;

    /// <inheritdoc />
    public class TabService : ITabService
    {
        private readonly IRibbonComponentStorageService _storageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabService"/> class.
        /// </summary>
        /// <param name="storageService"><see cref="IRibbonComponentStorageService"/>.</param>
        public TabService(IRibbonComponentStorageService storageService)
        {
            _storageService = storageService;
        }

        /// <inheritdoc />
        public RibbonTab? GetTab(string tabName)
        {
            return ComponentManager.Ribbon.Tabs.FirstOrDefault(x =>
                x.IsVisible && x.Title != null && x.Title.Equals(tabName, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public RibbonTab CreateTab(string tabName)
        {
            var tab = new RibbonTab
            {
                Title = tabName,
                Id = $"TAB_{tabName.GetHashCode():0}"
            };

            ComponentManager.Ribbon.Tabs.Add(tab);
            _storageService.AddTab(tab);

            return tab;
        }
    }
}