namespace RxBim.Shared.Abstractions
{
    using System.Collections.Generic;
    using System.Reflection;
    using Di;

    /// <summary>
    /// Factory for strategies.
    /// </summary>
    /// <typeparam name="T">Strategy type.</typeparam>
    public interface IStrategyFactory<out T>
    {
        /// <summary>
        /// Returns strategies.
        /// </summary>
        IEnumerable<T> GetStrategies();

        /// <summary>
        /// Registers the strategies from the assembly in the container.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        void AddStrategies(Assembly assembly);
    }
}