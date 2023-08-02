namespace RxBim.Di.Tests;

extern alias msdi;
using System;
using FluentAssertions;
using msdi::Microsoft.Extensions.DependencyInjection;
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
        Action act = () =>
        {
            using var provider = testDiConfigurator.Services.BuildServiceProvider(false);
            provider.GetRequiredService<IBaseService>();
            provider.GetRequiredService<IPluginService>();
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
        Action act = () => result = methodCaller.InvokeMethod(testDiConfigurator.Services, "Execute");

        act.Should().NotThrow();
        result.Should().Be(100);
    }
}