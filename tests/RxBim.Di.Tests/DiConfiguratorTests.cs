namespace RxBim.Di.Tests;

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
        var sp = testDiConfigurator.Build();
        var act = () =>
        {
            sp.GetService<IBaseService>();
            sp.GetService<IPluginService>();
        };
        act.Should().NotThrow();
    }

    [Fact]
    public void AllDependenciesShouldBeInjected()
    {
        var testDiConfigurator = new TestDiConfigurator();
        testDiConfigurator.Configure(GetType().Assembly);
        var sp = testDiConfigurator.Build();
        var methodCaller = new MethodCaller<int>(new ObjectWithDependencies());

        var result = 0;
        Action act = () =>
            result = methodCaller.InvokeMethod(sp, "Execute");

        act.Should().NotThrow();
        result.Should().Be(100);
    }
}