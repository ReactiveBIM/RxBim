namespace RxBim.Application.Ribbon
{
    using System;
    using RxBim.Shared;

    /// <summary>
    /// Defines a ribbon tab.
    /// </summary>
    public interface ITabBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Adds a new panel to the tab.
        /// </summary>
        /// <param name="title">Panel name.</param>
        /// <param name="panel">The panel configurator.</param>
        ITabBuilder AddPanel(string title, Action<IPanelBuilder> panel);

        /// <summary>
        /// Adds a new "About" button panel to the tab.
        /// </summary>
        /// <param name="name">The button name.</param>
        /// <param name="content">The About window content.</param>
        /// <param name="builder">The "About" button builder.</param>
        /// <param name="panelName">The panel name.</param>
        ITabBuilder AddAboutButton(
            string name,
            AboutBoxContent content,
            Action<IButtonBuilder>? builder = null,
            string? panelName = null);
    }
}