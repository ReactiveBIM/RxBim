namespace RxBim.Application.Ribbon.Abstractions
{
    using ConfigurationBuilders;

    /// <summary>
    /// Strategy for adding item to the ribbon.
    /// </summary>
    public interface IAddElementStrategy
    {
        /// <summary>
        /// Returns true if the strategy is applicable for specified item configuration. Otherwise returns false.
        /// </summary>
        /// <param name="config">Ribbon item configuration.</param>
        bool IsApplicable(IRibbonPanelElement config);

        /// <summary>
        /// Creates and adds to ribbon an element.
        /// </summary>
        /// <param name="tab">Ribbon tab.</param>
        /// <param name="panel">Ribbon panel.</param>
        /// <param name="config">Ribbon item configuration.</param>
        void CreateAndAddElement(object tab, object panel, IRibbonPanelElement config);

        /// <summary>
        /// Creates and returns an element for a stack.
        /// </summary>
        /// <param name="config">Ribbon item configuration.</param>
        /// <param name="small">Element is small.</param>
        object CreateElementForStack(IRibbonPanelElement config, bool small = false);
    }
}