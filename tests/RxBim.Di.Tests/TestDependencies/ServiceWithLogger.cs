namespace RxBim.Di.Tests.TestDependencies
{
    using Serilog;

    public class ServiceWithLogger
    {
        private readonly ILogger _logger;

        public ServiceWithLogger(ILogger logger)
        {
            _logger = logger;
        }
    }
}