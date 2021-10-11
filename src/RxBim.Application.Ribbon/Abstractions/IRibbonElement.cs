namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon element
    /// </summary>
    public interface IRibbonElement
    {
        /// <summary>
        /// Unique identifier of the ribbon element
        /// </summary>
        public string Id { get; }
    }
}