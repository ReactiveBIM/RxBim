namespace RxBim.Application.Ribbon.Autocad.Models
{
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
        public override bool IsEnabled => ComponentManager.Ribbon != null;

        /// <inheritdoc />
        protected override bool TabIsExists(string tabTitle)
        {
            var ribbonTab = ComponentManager.Ribbon?.Tabs.FirstOrDefault(t => t.Title.Equals(tabTitle));
            return ribbonTab != null;
        }

        /// <inheritdoc />
        protected override void CreateTabAndAddToRibbon(string tabTitle)
        {
            var newId = $"TAB_{tabTitle.GetHashCode()}";
            var tab = new RibbonTab
            {
                Title = tabTitle,
                Name = tabTitle,
                Id = newId
            };
            ComponentManager.Ribbon?.Tabs.Add(tab);
        }

        /// <inheritdoc />
        protected override ITab CreateTab(string title)
        {
            var ribbonTab = ComponentManager.Ribbon.Tabs.Single(t => t.Title.Equals(title));
            return new Tab(this, ribbonTab.Id, Container);
        }
    }
}