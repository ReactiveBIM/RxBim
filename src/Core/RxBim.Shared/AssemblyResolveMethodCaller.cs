namespace RxBim.Shared
{
    using Di;

    /// <inheritdoc />
    public class AssemblyResolveMethodCaller<T> : MethodCallerDecorator<T>
    {
        private readonly AssemblyResolver _resolver;

        /// <inheritdoc />
        public AssemblyResolveMethodCaller(IMethodCaller<T> decorated, AssemblyResolver resolver)
            : base(decorated)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public override T InvokeMethod(IContainer container, string methodName)
        {
            var result = Decorated.InvokeMethod(container, methodName);

            if (methodName == Constants.ShutdownMethodName)
            {
                _resolver.Dispose();
            }

            return result;
        }
    }
}