﻿namespace RxBim.Di
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public abstract class MethodCallerDecorator<T> : IMethodCaller<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCallerDecorator{T}"/> class.
        /// </summary>
        /// <param name="decorated">Decorated object.</param>
        protected MethodCallerDecorator(IMethodCaller<T> decorated)
        {
            Decorated = decorated;
        }

        /// <inheritdoc />
        public Type SourceObjectType => Decorated.SourceObjectType;

        /// <summary>
        /// Decorated object.
        /// </summary>
        protected IMethodCaller<T> Decorated { get; }

        /// <inheritdoc />
        public abstract T InvokeMethod(IServiceCollection services, string methodName);
    }
}