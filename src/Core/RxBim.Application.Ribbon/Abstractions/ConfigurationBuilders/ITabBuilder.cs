namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Defines a ribbon tab.
    /// </summary>
    public interface ITabBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Building tab
        /// </summary>
        Tab BuildingTab { get; }

        /// <summary>
        /// The parent builder for the ribbon.
        /// </summary>
        IRibbonBuilder RibbonBuilder { get; }

        /// <summary>
        /// Adds a new panel to the tab.
        /// </summary>
        /// <param name="title">Panel name.</param>
        /// <param name="panel">The panel configurator.</param>
        ITabBuilder Panel(string title, Action<IPanelBuilder> panel);
    }
}