namespace PikTools.Logs
{
    using System;
    using System.Diagnostics;
    using Di;
    using Serilog;
    using SimpleInjector;

    /// <inheritdoc />
    public class LoggedMethodCaller<T> : IMethodCaller<T>
    {
        private readonly IMethodCaller<T> _decorated;
        private readonly ILogger _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="decorated">исзодный объект</param>
        /// <param name="logger">логгер</param>
        public LoggedMethodCaller(IMethodCaller<T> decorated, ILogger logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        /// <inheritdoc />
        public T InvokeCommand(Container container, string methodName)
        {
            _logger.Debug("Started");
            var result = _decorated.InvokeCommand(container, methodName);
            _logger.Debug("Completed");

            return result;
        }
    }
}