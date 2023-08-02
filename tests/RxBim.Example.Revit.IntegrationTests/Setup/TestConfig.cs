namespace RxBim.Example.Revit.IntegrationTests.Setup
{
    using Di.Testing.Revit;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class TestConfig : ITestConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<ITestService, TestService>();
        }
    }
}