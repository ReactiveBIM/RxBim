namespace RxBim.Application.Ribbon.Models
{
    using Abstractions;
    using Di;

    /// <summary>
    /// Ribbon builder
    /// </summary>
    public abstract class RibbonBuilderBase<T> : IRibbonBuilder
        where T : IRibbon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonBuilderBase{T}"/> class.
        /// </summary>
        /// <param name="ribbon">Parent ribbon object</param>
        /// <param name="container">DI container</param>
        protected RibbonBuilderBase(T ribbon, IContainer container)
        {
            Ribbon = ribbon;
            Container = container;
        }

        /// <summary>
        /// Parent ribbon
        /// </summary>
        protected T Ribbon { get; }

        /// <summary>
        /// DI-container
        /// </summary>
        protected IContainer Container { get; }

        /// <summary>
        /// Returns parent ribbon
        /// </summary>
        public IRibbon And()
        {
            return Ribbon;
        }
    }
}