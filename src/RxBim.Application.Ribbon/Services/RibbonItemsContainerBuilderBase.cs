namespace RxBim.Application.Ribbon.Services
{
    using Abstractions;
    using Di;

    /// <summary>
    /// Ribbon builder
    /// </summary>
    public abstract class RibbonItemsContainerBuilderBase<T> : IRibbonItemsContainerBuilder
        where T : IRibbonBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonItemsContainerBuilderBase{T}"/> class.
        /// </summary>
        /// <param name="ribbonBuilder">Parent ribbon object</param>
        /// <param name="container">DI container</param>
        protected RibbonItemsContainerBuilderBase(T ribbonBuilder, IContainer container)
        {
            RibbonBuilder = ribbonBuilder;
            Container = container;
        }

        /// <summary>
        /// Parent ribbon
        /// </summary>
        protected T RibbonBuilder { get; }

        /// <summary>
        /// DI-container
        /// </summary>
        protected IContainer Container { get; }

        /// <summary>
        /// Returns parent ribbon
        /// </summary>
        public IRibbonBuilder And()
        {
            return RibbonBuilder;
        }
    }
}