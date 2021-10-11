namespace RxBim.Application.Ribbon.Models
{
    using System;
    using System.Collections.Generic;
    using Abstractions;
    using Di;

    /// <inheritdoc />
    public abstract class RibbonBase : IRibbon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonBase"/> class.
        /// </summary>
        /// <param name="container">DI-container</param>
        protected RibbonBase(IContainer container)
        {
            Container = container;
        }

        /// <inheritdoc />
        public abstract bool RibbonIsOn { get; }

        /// <summary>
        /// Tabs
        /// </summary>
        public Dictionary<string, ITab> Tabs { get; } = new ();

        /// <summary>
        /// DI-container
        /// </summary>
        protected IContainer Container { get; }

        /// <inheritdoc />
        public ITab Tab(string tabTitle)
        {
            if (string.IsNullOrEmpty(tabTitle))
            {
                throw new InvalidOperationException("Tab title is not set!");
            }

            if (!TabIsExists(tabTitle, out var tabId))
            {
                tabId = CreateTabAndAddToRibbon(tabTitle);
            }

            return GetTab(tabTitle, tabId);
        }

        /// <summary>
        /// Returns true if the tab with a title exists, otherwise returns false
        /// </summary>
        /// <param name="tabTitle">Title of a tab</param>
        /// <param name="tabId">Existing tab identifier</param>
        protected abstract bool TabIsExists(string tabTitle, out string tabId);

        /// <summary>
        /// Создаёт вкладку и добавляет её на ленту
        /// </summary>
        /// <param name="tabTitle">Название вкладки</param>
        /// <returns>New tab identifier</returns>
        protected abstract string CreateTabAndAddToRibbon(string tabTitle);

        /// <summary>
        /// Возвращает вкладку
        /// </summary>
        /// <param name="title">Название вкладки</param>
        /// <param name="id">Tab identifier</param>
        protected abstract ITab CreateTab(string title, string id);

        private ITab GetTab(string title, string id)
        {
            if (!Tabs.TryGetValue(title, out var tab))
            {
                tab = CreateTab(title, id);
                Tabs[title] = tab;
            }

            return tab;
        }
    }
}