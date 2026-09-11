namespace RxBim.Shared.Tests;

using System;
using Xunit;
using static TestPluginDirectory;

public class AutocadCommandContextTests
{
    [Fact]
    public void IsolationAndReuseRemainDisabledByDefault()
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "DefaultAutocadCommand");
        var command = Activator.CreateInstance(type)!;

        Assert.False(ReadProperty<bool>(command, "DefaultRunInSeparatedContext"));
        Assert.False(ReadProperty<bool>(command, "DefaultReuseSeparatedContext"));
    }

    [Fact]
    public void CommandsExecuteInTheReusedContextDespiteDifferentBaseTypeIdentity()
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "ReusedAutocadCommand");
        var sourceCommand = Activator.CreateInstance(type)!;

        Execute(sourceCommand);
        Execute(Activator.CreateInstance(type)!);

        var isolatedCommand = PluginContext.CreateInstanceInReusedContext(type);
        Assert.False(type.BaseType!.IsInstanceOfType(isolatedCommand));
        Assert.Equal(0, ReadProperty<int>(sourceCommand, "ExecutionCount"));
        Assert.Equal(2, ReadProperty<int>(isolatedCommand, "ExecutionCount"));
        Assert.Empty(ReadProperty<string>(sourceCommand, "LastError"));
    }

    [Fact]
    public void ActivationFailureReportsTheErrorWithoutExecutingInTheOriginalContext()
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "FailingAutocadCommand");
        var command = Activator.CreateInstance(type)!;

        Execute(command);

        Assert.Equal(0, ReadProperty<int>(command, "ExecutionCount"));
        Assert.Contains("Isolated activation failed.", ReadProperty<string>(command, "LastError"));
    }

    [Fact]
    public void ExecutionFailurePropagatesWithoutBeingReportedAsAnActivationError()
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "ThrowingAutocadCommand");
        var command = Activator.CreateInstance(type)!;

        var exception = Assert.Throws<InvalidOperationException>(() => Execute(command));

        Assert.Equal("Command execution failed.", exception.Message);
        Assert.Empty(ReadProperty<string>(command, "LastError"));
    }

    private static void Execute(object command) =>
        ((Action)Delegate.CreateDelegate(typeof(Action), command, "Execute"))();
}