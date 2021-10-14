namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using Di;
    using Ribbon.Services;
    using UIFramework;

    /// <summary>
    /// Ribbon wrapper
    /// </summary>
    public class RibbonBuilder : Ribbon.Services.RibbonBuilder
    {
        private readonly RibbonControl _ribbonControl;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="application">UIControlledApplication</param>
        /// <param name="container"><see cref="IContainer"/></param>
        public RibbonBuilder(UIControlledApplication application, IContainer container)
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
        protected override ITabBuilder CreateTab(string title)
        {
            return new TabBuilder(this, title, Container);
        }
    }
}