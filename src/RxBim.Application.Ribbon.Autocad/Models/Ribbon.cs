namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using Application.Ribbon.Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Windows;
    using Di;

    /// <inheritdoc />
    public class Ribbon : RibbonBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ribbon"/> class.
        /// </summary>
        /// <param name="container">DI container</param>
        public Ribbon(IContainer container)
            : base(container)
        {
        }

        /// <inheritdoc />
        public override bool RibbonIsOn => ComponentManager.Ribbon != null;

        /// <inheritdoc />
        protected override bool TabIsExists(string tabTitle, out string tabId)
        {
            var ribbonTab = ComponentManager.Ribbon?.Tabs.FirstOrDefault(t => t.Title.Equals(tabTitle));
            tabId = ribbonTab?.Id ?? string.Empty;
            return ribbonTab != null;
        }

        /// <inheritdoc />
        protected override string CreateTabAndAddToRibbon(string tabTitle)
        {
            var newId = $"TAB_{Guid.NewGuid()}";
            var tab = new RibbonTab
            {
                Title = tabTitle,
                Name = tabTitle,
                Id = newId
            };
            ComponentManager.Ribbon?.Tabs.Add(tab);
            return newId;
        }

        /// <inheritdoc />
        protected override ITab CreateTab(string title, string id)
        {
            return new Tab(this, title, id, Container);
        }
    }
}