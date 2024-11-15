namespace RxBim.Logs
{
    using System;
    using Di;
    using Serilog;

    /// <inheritdoc />
    public class LoggedMethodCaller<T> : MethodCallerDecorator<T>
    {
        private readonly ILogger _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="decorated">Decorated method caller.</param>
        /// <param name="logger">A logger.</param>
        public LoggedMethodCaller(IMethodCaller<T> decorated, ILogger logger)
            : base(decorated)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override T InvokeMethod(IServiceProvider serviceProvider, string methodName)
        {
            T result;
            try
            {
                result = Decorated.InvokeMethod(serviceProvider, methodName);
            }
            catch (Exception e)
            {
                _logger.Error(e,
                    "Error was thrown at execution of method {MethodName} of object {TypeName}",
                    methodName,
                    SourceObjectType.FullName);
                throw;
            }

            _logger.Information("Method {MethodName} of object {TypeName} executed successfully",
                methodName,
                SourceObjectType.FullName);

            return result;
        }
    }
}