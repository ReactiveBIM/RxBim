namespace RxBim.Application.Ribbon.Abstractions
{
    using System;

    /// <summary>
    /// Ribbon tab
    /// </summary>
    public interface ITabBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Creates and returns a panel on this tab
        /// </summary>
        /// <param name="panelTitle">Panel name</param>
        IPanelBuilder Panel(string panelTitle);

        /// <summary>
        /// Creates "About" button on the own panel
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="action">"About" button additional actions</param>
        /// <param name="panelName">Panel name</param>
        /// <param name="text">Button label text</param>
        ITabBuilder AddAboutButton(
            string name,
            Action<IAboutButtonBuilder> action,
            string? panelName = null,
            string? text = null);
    }
}