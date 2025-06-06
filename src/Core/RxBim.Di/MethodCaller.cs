﻿namespace RxBim.Di
{
    using System;
    using System.Linq;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class MethodCaller<T> : IMethodCaller<T>
    {
        private readonly object _sourceObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCaller{T}"/> class.
        /// </summary>
        /// <param name="sourceObject">The source object for the method.</param>
        public MethodCaller(object sourceObject)
        {
            _sourceObject = sourceObject;
        }

        /// <inheritdoc />
        public Type SourceObjectType => _sourceObject.GetType();

        /// <inheritdoc />
        public T InvokeMethod(IServiceProvider serviceProvider, string methodName)
        {
            var methodInfo = _sourceObject.GetType().FindInvokeMethod<T>(methodName);

            if (!methodInfo.GetParameters().Any())
            {
                return methodInfo.Invoke<T>(_sourceObject);
            }

            var parameters = methodInfo.GetMethodParameters(serviceProvider);
            return methodInfo.Invoke<T>(_sourceObject, parameters.ToArray());
        }
    }
}