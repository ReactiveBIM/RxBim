namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using System.Linq;
    using Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using Di;
    using UIFramework;

    /// <summary>
    /// Ribbon wrapper
    /// </summary>
    public class Ribbon : RibbonBase
    {
        private readonly RibbonControl _ribbonControl;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="application">UIControlledApplication</param>
        /// <param name="container"><see cref="IContainer"/></param>
        public Ribbon(UIControlledApplication application, IContainer container)
            : base(container)
        {
            Application = application;

            _ribbonControl = RevitRibbonControl.RibbonControl;
            if (_ribbonControl == null)
                throw new NotSupportedException("Could not initialize Revit ribbon control");
        }

        /// <summary>
        /// UIControlledApplication
        /// </summary>
        public UIControlledApplication Application { get; }

        /// <inheritdoc />
        public override bool IsEnabled => _ribbonControl != null;

        /// <inheritdoc />
        protected override bool TabIsExists(string tabTitle)
        {
            return _ribbonControl.Tabs.Any(t => t.Title.Equals(tabTitle));
        }

        /// <inheritdoc />
        protected override void CreateTabAndAddToRibbon(string tabTitle)
        {
            Application.CreateRibbonTab(tabTitle);
        }

        /// <inheritdoc />
        protected override ITab CreateTab(string title)
        {
            return new Tab(this, title, Container);
        }
    }
}