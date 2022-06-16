namespace RxBim.Application.Ribbon
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting an item from a configuration section.
    /// </summary>
    public interface IItemFromConfigStrategy
    {
        /// <summary>
        /// Returns true if strategy is applicable for this configuration section. Otherwise returns false.
        /// </summary>
        /// <param name="itemSection">Configuration section.</param>
        bool IsApplicable(IConfigurationSection itemSection);

        /// <summary>
        /// Creates an item and adds to a panel.
        /// </summary>
        /// <param name="itemSection">Element configuration section.</param>
        /// <param name="panelBuilder">Panel builder.</param>
        void CreateAndAddToPanelConfig(IConfigurationSection itemSection, IPanelBuilder panelBuilder);

        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="itemSection">Element configuration section.</param>
        IRibbonPanelItem CreateForStack(IConfigurationSection itemSection);
    }
}