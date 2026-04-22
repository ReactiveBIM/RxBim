namespace RxBim.Di;

using System;

/// <summary>
/// A method caller. Calls wrapped method.
/// </summary>
/// <typeparam name="T">The return type of the method.</typeparam>
public interface IMethodCaller<out T>
{
    /// <summary>
    /// The type of the original object to call the method.
    /// </summary>
    Type SourceObjectType { get; }

    /// <summary>
    /// Returns the result of a method call.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> object.</param>
    /// <param name="methodName">The method name.</param>
    T InvokeMethod(IServiceProvider serviceProvider, string methodName);
}