namespace RxBim.Shared.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Factory for items collection.
    /// </summary>
    /// <typeparam name="T">Item interface type.</typeparam>
    public interface IDiCollectionService<out T>
    {
        /// <summary>
        /// Returns items collection.
        /// </summary>
        IEnumerable<T> GetItems();
    }
}