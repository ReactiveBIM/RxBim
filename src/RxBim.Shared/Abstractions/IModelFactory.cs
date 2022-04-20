namespace RxBim.Shared.Abstractions
{
    using System;

    /// <summary>
    /// Models factory abstraction.
    /// </summary>
    public interface IModelFactory
    {
        /// <summary>
        /// Creates a new instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the requested instance.</typeparam>
        /// <returns>A new instance of the requested instance.</returns>
        T GetInstance<T>()
            where T : class;

        /// <summary>
        /// Creates a new instance of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The requested instance type.</param>
        /// <returns>An instance object of the requested type.</returns>
        object GetInstance(Type type);
    }
}
