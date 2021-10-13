namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System.Linq;
    using Application.Ribbon.Abstractions;
    using Autodesk.Windows;
    using Di;
    using Ribbon.Services;

    /// <inheritdoc />
    public class RibbonBuilder : RibbonBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonBuilder"/> class.
        /// </summary>
        /// <param name="container">DI container</param>
        public RibbonBuilder(IContainer container)
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
        protected override ITabBuilder CreateTab(string title)
        {
            var ribbonTab = ComponentManager.Ribbon.Tabs.Single(t => t.Title.Equals(title));
            return new TabBuilder(this, ribbonTab.Id, Container);
        }
    }
}