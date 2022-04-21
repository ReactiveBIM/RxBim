namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Defines a ribbon builder.
    /// </summary>
    public interface IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Finishes the ribbon item building.
        /// </summary>
        IRibbonBuilder ReturnToRibbon();
    }
}