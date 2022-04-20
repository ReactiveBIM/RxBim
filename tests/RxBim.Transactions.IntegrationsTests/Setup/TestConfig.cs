namespace RxBim.Transactions.IntegrationsTests.Setup
{
    using Di;
    using Di.Testing.Revit;
    using Revit.Extensions;

    /// <inheritdoc />
    public class TestConfig : ITestConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddTransient<ITestService, TestService>();
            container.AddTransactions();
        }
    }
}