namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    /// <summary>
    /// Ribbon builder
    /// </summary>
    public interface IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Returns ribbon builder object
        /// </summary>
        IRibbonBuilder ToRibbonBuilder();
    }
}