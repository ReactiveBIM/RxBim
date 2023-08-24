namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Service locator.
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

        /// <summary>
        /// Returns all services that can be assigned to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to assign.</typeparam>
        IEnumerable<T> GetServicesAssignableTo<T>();

        /// <summary>
        /// Returns all services that can be assigned to <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to assign.</param>
        IEnumerable<object> GetServicesAssignableTo(Type type);
    }
}