namespace RxBim.Di.Tests;

extern alias msdi;
using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
        var act = () =>
        {
            testDiConfigurator.Container.ServiceProvider.GetRequiredService<IBaseService>();
            testDiConfigurator.Container.ServiceProvider.GetRequiredService<IPluginService>();
        };
        act.Should().NotThrow();
    }

    [Fact]
    public void AllDependenciesShouldBeInjected()
    {
        var testDiConfigurator = new TestDiConfigurator();
        testDiConfigurator.Configure(GetType().Assembly);
        var methodCaller = new MethodCaller<int>(new ObjectWithDependencies());

        var result = 0;
        Action act = () => result = methodCaller.InvokeMethod(testDiConfigurator.Container, "Execute");

        act.Should().NotThrow();
        result.Should().Be(100);
    }
}