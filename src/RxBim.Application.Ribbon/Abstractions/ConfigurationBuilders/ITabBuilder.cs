namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using Models.Configurations;

    /// <summary>
    /// Ribbon tab builder.
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
        /// Creates and returns a panel on this tab
        /// </summary>
        /// <param name="panelTitle">Panel name</param>
        IPanelBuilder AddPanel(string panelTitle);
    }
}