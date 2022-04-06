namespace RxBim.Application.Ribbon.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Factory for strategies.
    /// </summary>
    /// <typeparam name="T">Strategy type.</typeparam>
    public interface IStrategiesFactory<out T>
    {
        /// <summary>
        /// Returns strategies.
        /// </summary>
        IEnumerable<T> GetStrategies();
    }
}