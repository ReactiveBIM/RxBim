namespace PikTools.Application.Api
{
    using Di;
    using Shared;
    using SimpleInjector;

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
        public override PluginResult InvokeCommand(Container container, string methodName)
        {
            var result = Decorated.InvokeCommand(container, methodName);

            if (methodName == Constants.ShutdownMethodName)
            {
                _resolver.Dispose();
            }

            return result;
        }
    }
}