namespace RxBim.Shared
{
    using Di;

    /// <inheritdoc />
    public class AssemblyResolveMethodCaller : MethodCallerDecorator<PluginResult>
    {
        private readonly AssemblyResolver _resolver;

        /// <inheritdoc />
        public AssemblyResolveMethodCaller(IMethodCaller<PluginResult> decorated, AssemblyResolver resolver)
            : base(decorated)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public override PluginResult InvokeMethod(IContainer container, string methodName)
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