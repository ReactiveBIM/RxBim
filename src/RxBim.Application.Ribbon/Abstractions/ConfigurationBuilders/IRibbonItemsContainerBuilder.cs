namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    /// <summary>
    /// The ribbon builder.
    /// </summary>
    public interface IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Returns the ribbon builder object.
        /// </summary>
        IRibbonBuilder ReturnToRibbon();
    }
}