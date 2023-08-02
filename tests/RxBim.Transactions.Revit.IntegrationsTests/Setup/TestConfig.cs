namespace RxBim.Transactions.Revit.IntegrationsTests.Setup
{
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