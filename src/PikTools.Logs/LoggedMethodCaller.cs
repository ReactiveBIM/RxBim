namespace PikTools.Logs
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
        /// <param name="decorated">исзодный объект</param>
        /// <param name="logger">логгер</param>
        public LoggedMethodCaller(IMethodCaller<T> decorated, ILogger logger)
            : base(decorated)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override T InvokeCommand(IContainer container, string methodName)
        {
            T result;
            try
            {
                result = Decorated.InvokeCommand(container, methodName);
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