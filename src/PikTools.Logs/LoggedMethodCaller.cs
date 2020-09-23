namespace PikTools.Logs
{
    using System;
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
        public Type SourceObjectType => _decorated.SourceObjectType;

        /// <inheritdoc />
        public T InvokeCommand(Container container, string methodName)
        {
            T result;
            try
            {
                result = _decorated.InvokeCommand(container, methodName);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Ошибка выполнения метода {methodName} объекта {SourceObjectType.FullName}.");
                throw;
            }

            _logger.Information($"Метод {methodName} объекта {SourceObjectType.FullName} выполнен.");

            return result;
        }
    }
}