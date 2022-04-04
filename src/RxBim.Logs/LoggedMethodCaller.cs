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
        /// ctor
        /// </summary>
        /// <param name="decorated">Decorated method caller.</param>
        /// <param name="logger">A logger.</param>
        public LoggedMethodCaller(IMethodCaller<T> decorated, ILogger logger)
            : base(decorated)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override T InvokeMethod(IContainer container, string methodName)
        {
            T result;
            try
            {
                result = Decorated.InvokeMethod(container, methodName);
            }
            catch (Exception e)
            {
                _logger.Error(e,
                    $"Error was thrown at execution of method {methodName} of object {SourceObjectType.FullName}.");
                throw;
            }

            _logger.Information($"Method {methodName} of object {SourceObjectType.FullName} executed successfully.");

            return result;
        }
    }
}