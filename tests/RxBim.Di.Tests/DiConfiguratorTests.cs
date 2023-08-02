namespace RxBim.Di.Tests
{
    using System;
    using FluentAssertions;
    using TestDependencies;
    using TestObjects;
    using Xunit;

    public class DiConfiguratorTests
    {
        [Fact]
        public void AllDependenciesShouldBeResolved()
        {
            var testDiConfigurator = new TestDiConfigurator();
            testDiConfigurator.Configure(GetType().Assembly);
            Action act = () =>
            {
                testDiConfigurator.Services.GetService<IBaseService>();
                testDiConfigurator.Services.GetService<IPluginService>();
            };
            act.Should().NotThrow();
        }

        [Fact]
        public void AllDependenciesShouldBeInjected()
        {
            var testDiConfigurator = new TestDiConfigurator();
            testDiConfigurator.Configure(GetType().Assembly);
            var methodCaller = new MethodCaller<int>(new ObjectWithDependencies());

            int result = 0;
            Action act = () => result = methodCaller.InvokeMethod(testDiConfigurator.Services, "Execute");

            act.Should().NotThrow();
            result.Should().Be(100);
        }
    }
}