namespace RxBim.Application.Ribbon.Services
{
    using Abstractions;
    using Models;

    /// <inheritdoc />
    public class RibbonControlBuilder<T> : IRibbonControlBuilder
    where T : RibbonControl, new()
    {
        /// <summary>
        /// Building ribbon control
        /// </summary>
        public T Control { get; } = new ();

        /// <inheritdoc />
        public IRibbonControlBuilder SetId(string id)
        {
            Control.Id = id;
        }
    }
}