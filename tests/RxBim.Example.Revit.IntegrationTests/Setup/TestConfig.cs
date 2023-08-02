namespace RxBim.Example.Revit.IntegrationTests.Setup
{
    using Di.Testing.Revit;

    /// <inheritdoc />
    public class TestConfig : ITestConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddTransient<ITestService, TestService>();
        }
    }
}