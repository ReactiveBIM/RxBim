namespace RxBim.Application.Ribbon.Abstractions
{
    using System;
    using Di;

    /// <summary>
    /// Ribbon Factory
    /// </summary>
    public interface IRibbonFactory
    {
        /// <summary>
        /// Creates and returns a ribbon
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="action">Action to build a ribbon</param>
        IRibbonBuilder Create(IContainer container, Action<IRibbonBuilder> action);
    }
}