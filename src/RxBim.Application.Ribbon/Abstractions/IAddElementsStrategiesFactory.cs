namespace RxBim.Application.Ribbon.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Factory for strategies for adding items to the ribbon.
    /// </summary>
    public interface IAddElementsStrategiesFactory
    {
        /// <summary>
        /// Returns strategies for adding items to the ribbon.
        /// </summary>
        IEnumerable<IAddElementStrategy> GetStrategies();
    }
}