namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Collections.Generic;
    using Abstractions;
    using Di;

    /// <inheritdoc />
    public abstract class RibbonBuilderBase : IRibbonBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonBuilderBase"/> class.
        /// </summary>
        /// <param name="container">DI container</param>
        protected RibbonBuilderBase(IContainer container)
        {
            Container = container;
        }

        /// <inheritdoc />
        public abstract bool IsEnabled { get; }

        /// <summary>
        /// Tabs
        /// </summary>
        public Dictionary<string, ITabBuilder> Tabs { get; } = new ();

        /// <summary>
        /// DI-container
        /// </summary>
        protected IContainer Container { get; }

        /// <inheritdoc />
        public ITabBuilder AddTab(string tabTitle)
        {
            if (string.IsNullOrEmpty(tabTitle))
            {
                throw new InvalidOperationException("Tab title is not set!");
            }

            if (!TabIsExists(tabTitle))
            {
                CreateTabAndAddToRibbon(tabTitle);
            }

            return GetTab(tabTitle);
        }

        /// <summary>
        /// Returns true if the tab with a title exists, otherwise returns false
        /// </summary>
        /// <param name="tabTitle">Title of a tab</param>
        protected abstract bool TabIsExists(string tabTitle);

        /// <summary>
        /// Создаёт вкладку и добавляет её на ленту
        /// </summary>
        /// <param name="tabTitle">Название вкладки</param>
        protected abstract void CreateTabAndAddToRibbon(string tabTitle);

        /// <summary>
        /// Возвращает вкладку
        /// </summary>
        /// <param name="title">Название вкладки</param>
        protected abstract ITabBuilder CreateTab(string title);

        private ITabBuilder GetTab(string title)
        {
            if (!Tabs.TryGetValue(title, out var tab))
            {
                tab = CreateTab(title);
                Tabs[title] = tab;
            }

            return tab;
        }
    }
}