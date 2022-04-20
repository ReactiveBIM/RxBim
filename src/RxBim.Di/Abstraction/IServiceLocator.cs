namespace RxBim.Di
{
    using System;

    /// <summary>
    /// Service locator
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Gets service with type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of requested service.</typeparam>
        /// <returns>An instance of requested service.</returns>
        T GetService<T>();

        /// <summary>
        /// Gets service with type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of requested service.</param>
        /// <returns>An instance of requested service.</returns>
        object GetService(Type type);
    }
}