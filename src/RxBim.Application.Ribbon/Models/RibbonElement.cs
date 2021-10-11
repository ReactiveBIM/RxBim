namespace RxBim.Application.Ribbon.Models
{
    using Abstractions;

    /// <inheritdoc />
    public abstract class RibbonElement : IRibbonElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonElement"/> class.
        /// </summary>
        /// <param name="id"><see cref="Id"/></param>
        protected RibbonElement(string id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public string Id { get; }
    }
}