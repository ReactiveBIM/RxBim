namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using System;
    using Shared;

    /// <summary>
    /// Defines a ribbon tab.
    /// </summary>
    public interface ITabBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Adds a new panel to the tab.
        /// </summary>
        /// <param name="panelTitle">Panel name.</param>
        IPanelBuilder AddPanel(string panelTitle);

        /// <summary>
        /// Adds a new "About" button panel the tab.
        /// </summary>
        /// <param name="name">The button name.</param>
        /// <param name="content">The About window content.</param>
        /// <param name="action">"About" button additional actions.</param>
        /// <param name="panelName">The panel name.</param>
        ITabBuilder AddAboutButton(
            string name,
            AboutBoxContent content,
            Action<IButtonBuilder>? action = null,
            string? panelName = null);
    }
}