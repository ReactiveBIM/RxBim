namespace RxBim.Application.Ribbon.Abstractions
{
    using Models;

    /// <summary>
    /// Builder for <see cref="RibbonControl"/>
    /// </summary>
    public interface IRibbonControlBuilder
    {
        /// <summary>
        /// Sets custom unique identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        IRibbonControlBuilder SetId(string id);
    }
}