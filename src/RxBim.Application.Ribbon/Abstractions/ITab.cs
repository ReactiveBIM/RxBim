namespace RxBim.Application.Ribbon.Abstractions
{
    using System;

    /// <summary>
    /// Ribbon tab
    /// </summary>
    public interface ITab : IRibbonBuilder
    {
        /// <summary>
        /// Creates a panel on this tab and returns it
        /// </summary>
        /// <param name="panelTitle">Panel name</param>
        IPanel Panel(string panelTitle);

        /// <summary>
        /// Creates "About" button
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="action">"About" button additional actions</param>
        /// <param name="panelName">Panel name</param>
        /// <param name="text">Button label text</param>
        ITab AboutButton(
            string name,
            Action<IAboutButton> action,
            string panelName = null,
            string text = null);
    }
}