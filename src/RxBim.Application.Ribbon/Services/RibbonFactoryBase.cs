namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Di;

    /// <inheritdoc />
    public abstract class RibbonFactoryBase : IRibbonFactory
    {
        /// <inheritdoc />
        public IRibbon Create(IContainer container, Action<IRibbon> action)
        {
            var ribbon = Create(container);
            if (ribbon.IsEnabled)
            {
                action(ribbon);
            }

            return ribbon;
        }

        /// <summary>
        /// Creates and returns a ribbon
        /// </summary>
        /// <param name="container">DI container</param>
        protected abstract IRibbon Create(IContainer container);
    }
}