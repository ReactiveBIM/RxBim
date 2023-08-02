namespace RxBim.Transactions.Revit.IntegrationsTests.Setup
{
    using Di.Testing.Revit;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class TestConfig : ITestConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<ITestService, TestService>();
            services.AddTransactions();
        }
    }
}