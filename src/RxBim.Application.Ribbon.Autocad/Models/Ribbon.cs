namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System.Globalization;
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
        /// <param name="container">Контейнер</param>
        public Ribbon(IContainer container)
        : base(container)
        {
            AcadRibbonControl = ComponentManager.Ribbon;
        }

        /// <summary>
        /// Лента AutoCAD
        /// </summary>
        public RibbonControl? AcadRibbonControl { get; }

        /// <inheritdoc />
        public override bool IsValid => AcadRibbonControl != null;

        /// <inheritdoc />
        protected override bool TabIsExists(string tabTitle)
        {
            return AcadRibbonControl?.Tabs.Any(t => t.Title.Equals(tabTitle)) ?? false;
        }

        /// <inheritdoc />
        protected override void CreateTabAndAddToRibbon(string tabTitle)
        {
            var tab = new RibbonTab
            {
                Title = tabTitle,
                Name = tabTitle,
                Id = $"TAB_{tabTitle.GetHashCode().ToString(CultureInfo.InvariantCulture)}"
            };
            AcadRibbonControl?.Tabs.Add(tab);
        }

        /// <inheritdoc />
        protected override ITab GetTab(string title, IContainer container)
        {
            return new Tab(this, title, container);
        }
    }
}