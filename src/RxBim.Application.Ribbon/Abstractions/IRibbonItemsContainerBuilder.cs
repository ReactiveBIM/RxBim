namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon builder
    /// </summary>
    public interface IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Returns ribbon object
        /// </summary>
        IRibbonBuilder And();
    }
}