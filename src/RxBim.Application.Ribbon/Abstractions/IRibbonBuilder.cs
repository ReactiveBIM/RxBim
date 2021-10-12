namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon builder
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Returns ribbon object
        /// </summary>
        IRibbon And();
    }
}