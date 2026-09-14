namespace RxBim.Shared.Tests.First;

using System;
using RxBim.Command.Autocad;
using Autodesk.AutoCAD.ApplicationServices.Core;

/// <summary>
/// Records dispatches through the real AutoCAD command base class in each context.
/// </summary>
public class ReusedAutocadCommand : RxBimCommand
{
    private static int _executionCount;

    /// <summary>
    /// The number of invocations in this context.
    /// </summary>
    public int ExecutionCount => _executionCount;

    /// <summary>
    /// The loading error reported by the command base class.
    /// </summary>
    public string LastError => Application.LastAlert;

    /// <inheritdoc />
    protected override bool RunInSeparatedContext => true;

    /// <inheritdoc />
    protected override bool ReuseSeparatedContext => true;

    /// <inheritdoc />
    public override void Execute()
    {
        if (PluginContext.IsCurrentContextRxBim(GetType()))
        {
            _executionCount++;
            return;
        }

        base.Execute();
    }
}

/// <summary>
/// Rejects activation only in the isolated context.
/// </summary>
public sealed class FailingAutocadCommand : ReusedAutocadCommand
{
    /// <summary>
    /// Creates the source command and rejects its isolated copy.
    /// </summary>
    public FailingAutocadCommand()
    {
        if (PluginContext.IsCurrentContextRxBim(GetType()))
            throw new InvalidOperationException("Isolated activation failed.");
    }
}

/// <summary>
/// Fails during execution after successfully entering the isolated context.
/// </summary>
public sealed class ThrowingAutocadCommand : ReusedAutocadCommand
{
    /// <inheritdoc />
    public override void Execute()
    {
        if (PluginContext.IsCurrentContextRxBim(GetType()))
            throw new InvalidOperationException("Command execution failed.");

        base.Execute();
    }
}

/// <summary>
/// Exposes the default isolation settings without executing host-dependent code.
/// </summary>
public sealed class DefaultAutocadCommand : RxBimCommand
{
    /// <summary>
    /// Whether isolated execution is enabled by default.
    /// </summary>
    public bool DefaultRunInSeparatedContext => RunInSeparatedContext;

    /// <summary>
    /// Whether context reuse is enabled by default.
    /// </summary>
    public bool DefaultReuseSeparatedContext => ReuseSeparatedContext;
}