namespace RxBim.Di
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A method caller. Calls wrapped method.
    /// </summary>
    /// <typeparam name="T">The return type of the method.</typeparam>
    public interface IMethodCaller<out T>
    {
        /// <summary>
        /// The type of the original object to call the method.
        /// </summary>
        public Type SourceObjectType { get; }

        /// <summary>
        /// Returns the result of a method call.
        /// </summary>
        /// <param name="services">A DI container.</param>
        /// <param name="methodName">The method name.</param>
        T InvokeMethod(IServiceCollection services, string methodName);
    }
}