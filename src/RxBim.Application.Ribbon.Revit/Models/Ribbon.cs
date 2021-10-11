namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using System.Linq;
    using Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Revit.UI;
    using Di;
    using UIFramework;

    /// <summary>
    /// Ribbon wrapper
    /// </summary>
    public class Ribbon : RibbonBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="application">UIControlledApplication</param>
        /// <param name="container"><see cref="IContainer"/></param>
        public Ribbon(UIControlledApplication application, IContainer container)
            : base(container)
        {
            Application = application;

            if (RevitRibbonControl.RibbonControl != null)
                throw new NotSupportedException("Could not initialize Revit ribbon control");
        }

        /// <summary>
        /// UIControlledApplication
        /// </summary>
        public UIControlledApplication Application { get; }

        /// <inheritdoc />
        public override bool RibbonIsOn => RevitRibbonControl.RibbonControl != null;

        /// <inheritdoc />
        protected override bool TabIsExists(string tabTitle, out string tabId)
        {
            var ribbonTab = RevitRibbonControl.RibbonControl.Tabs.FirstOrDefault(t => t.Title.Equals(tabTitle));
            tabId = ribbonTab?.Id;
            return ribbonTab != null;
        }

        /// <inheritdoc />
        protected override string CreateTabAndAddToRibbon(string tabTitle)
        {
            Application.CreateRibbonTab(tabTitle);
            return RevitRibbonControl.RibbonControl.Tabs.Single(t => t.Title == tabTitle).Id;
        }

        /// <inheritdoc />
        protected override ITab CreateTab(string title, string id)
        {
            return new Tab(this, title, id, Container);
        }
    }
}