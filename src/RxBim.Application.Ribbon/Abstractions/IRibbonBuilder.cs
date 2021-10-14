namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon builder
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Creates tab and adds to the ribbon
        /// </summary>
        /// <param name="tabTitle">Tab title</param>
        ITabBuilder AddTab(string tabTitle);
    }
}