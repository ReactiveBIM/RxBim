﻿namespace RxBim.Application.Ui.Autocad.Api.Models
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Autodesk.Windows;
    using Di;
    using RxBim.Application.Ui.Api.Abstractions;
    using RxBim.Application.Ui.Api.Models;

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
            if (AcadRibbonControl == null)
                throw new NotSupportedException("Could not initialize Autocad ribbon control");
        }

        /// <summary>
        /// Лента AutoCAD
        /// </summary>
        public RibbonControl AcadRibbonControl { get; }

        /// <inheritdoc />
        protected override bool TabIsExists(string tabTitle)
        {
            return AcadRibbonControl.Tabs.Any(t => t.Title.Equals(tabTitle));
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
            AcadRibbonControl.Tabs.Add(tab);
        }

        /// <inheritdoc />
        protected override ITab GetTab(string title, IContainer container)
        {
            return new Tab(this, title, container);
        }
    }
}