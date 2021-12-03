namespace RxBim.Di.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Reflection extensions
    /// </summary>
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Returns method parameters from a DI container
        /// </summary>
        /// <param name="methodInfo">Method info</param>
        /// <param name="container">DI container</param>
        /// <returns></returns>
        public static List<object> GetMethodParameters(this MethodBase methodInfo, IContainer container)
        {
            return methodInfo.GetParameters()
                .Select(parameterInfo => container.GetService(parameterInfo.ParameterType))
                .ToList();
        }

        /// <summary>
        /// Returns information about a method from an object type
        /// </summary>
        /// <param name="type">The source object type for the method</param>
        /// <param name="methodName">Method name</param>
        /// <typeparam name="T">The return type of the method</typeparam>
        /// <exception cref="MethodCallerException">
        /// Throws when the method is not found or the return value of the method is not <typeparamref name="T"/>
        /// </exception>
        public static MethodInfo FindInvokeMethod<T>(this Type type, string methodName)
        {
            var methodInfo = type.GetMethods()
                .SingleOrDefault(x => x.Name.Equals(methodName, StringComparison.CurrentCultureIgnoreCase));
            if (methodInfo == null)
            {
                throw new MethodCallerException($"Class '{type.FullName}' does not contain method '{methodName}'!");
            }

            if (methodInfo.ReturnType != typeof(T))
            {
                throw new MethodCallerException(
                    $"Method '{methodName}' has a return type other than '{typeof(T)}'!");
            }

            return methodInfo;
        }

        /// <summary>
        /// Returns the result of a method call
        /// </summary>
        /// <param name="methodInfo">Method info</param>
        /// <param name="sourceObject">Source object</param>
        /// <param name="parameters">Method parameters</param>
        /// <typeparam name="T">The return type of the method</typeparam>
        public static T Invoke<T>(
            this MethodBase methodInfo,
            object sourceObject,
            object[] parameters = null)
        {
            return (T)methodInfo.Invoke(sourceObject, parameters);
        }
    }
}