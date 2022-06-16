namespace RxBim.Application.Ribbon.Services
{
    using System.Collections.Generic;
    using Autodesk.Windows;

    /// <inheritdoc />
    internal class RibbonComponentStorageService : IRibbonComponentStorageService
    {
        private readonly Stack<RibbonTab> _tabs = new();

        private readonly Stack<RibbonPanel> _panels = new();

        private readonly Stack<(RibbonItem Item, RibbonItemCollection Collection)> _items = new();

        /// <inheritdoc />
        public void AddTab(RibbonTab tab)
        {
            _tabs.Push(tab);
        }

        /// <inheritdoc />
        public void AddPanel(RibbonPanel panel)
        {
            _panels.Push(panel);
        }

        /// <inheritdoc />
        public void AddItem(RibbonItem item, RibbonItemCollection collection)
        {
            _items.Push((item, collection));
        }

        /// <inheritdoc />
        public void DeleteComponents()
        {
            while (_items.Count > 0)
            {
                var (item, collection) = _items.Pop();
                collection.Remove(item);
            }

            while (_panels.Count > 0)
            {
                var panel = _panels.Pop();
                panel.Tab.Panels.Remove(panel);
            }

            while (_tabs.Count > 0)
            {
                var tab = _tabs.Pop();
                ComponentManager.Ribbon?.Tabs.Remove(tab);
            }
        }
    }
}