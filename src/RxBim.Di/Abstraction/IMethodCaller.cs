namespace RxBim.Di
{
    using System;

    /// <summary>
    /// MethodCaller
    /// </summary>
    /// <typeparam name="T">The return type of the method</typeparam>
    public interface IMethodCaller<out T>
    {
        /// <summary>
        /// The type of the original object to call the method
        /// </summary>
        public Type SourceObjectType { get; }

        /// <summary>
        /// Returns the result of a method call
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="methodName">Method name</param>
        T InvokeMethod(IContainer container, string methodName);
    }
}