namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using System;
    using Shared;

    /// <summary>
    /// Ribbon tab
    /// </summary>
    public interface ITabBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Creates and returns a panel on this tab
        /// </summary>
        /// <param name="panelTitle">Panel name</param>
        IPanelBuilder AddPanel(string panelTitle);

        /// <summary>
        /// Creates "About" button on the own panel
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="content">About window content</param>
        /// <param name="action">"About" button additional actions</param>
        /// <param name="panelName">Panel name</param>
        ITabBuilder AddAboutButton(
            string name,
            AboutBoxContent content,
            Action<IButtonBuilder>? action = null,
            string? panelName = null);
    }
}