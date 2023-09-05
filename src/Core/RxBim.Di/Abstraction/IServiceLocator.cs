namespace RxBim.Di;

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
    [Obsolete("Use GetServices<T>")]
    IEnumerable<T> GetServicesAssignableTo<T>();

    /// <summary>
    /// Returns all services that can be assigned to <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type to assign.</param>
    [Obsolete("Use GetServices")]
    IEnumerable<object> GetServicesAssignableTo(Type type);

    /// <summary>
    /// Returns all implementations of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Service type.</typeparam>
    IEnumerable<T> GetServices<T>();

    /// <summary>
    /// Returns all implementations of type <paramref name="type"/>.
    /// </summary>
    /// <param name="type">Service type.</param>
    IEnumerable<object> GetServices(Type type);
}