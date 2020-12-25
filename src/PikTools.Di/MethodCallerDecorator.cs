namespace PikTools.Di
{
    using System;
    using SimpleInjector;

    /// <inheritdoc />
    public abstract class MethodCallerDecorator<T> : IMethodCaller<T>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="decorated">декорируемый объект</param>
        protected MethodCallerDecorator(IMethodCaller<T> decorated)
        {
            Decorated = decorated;
        }

        /// <inheritdoc />
        public Type SourceObjectType => Decorated.SourceObjectType;

        /// <summary>
        /// декорируемый объект
        /// </summary>
        protected IMethodCaller<T> Decorated { get; }

        /// <inheritdoc />
        public abstract T InvokeCommand(Container container, string methodName);
    }
}