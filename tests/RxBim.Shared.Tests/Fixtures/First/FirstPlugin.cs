namespace RxBim.Shared.Tests.First;

using System;
using Dependency;

/// <summary>
/// The first entry point of the test application.
/// </summary>
public sealed class FirstPlugin
{
    /// <summary>
    /// The object's sequence number in the application's shared state.
    /// </summary>
    public int Sequence { get; } = Counter.Next();

    /// <summary>
    /// A type from the application's private dependency.
    /// </summary>
    public Type DependencyType => typeof(Counter);

    /// <summary>
    /// A type from the shared infrastructure assembly.
    /// </summary>
    public Type InfrastructureType => typeof(PluginContext);

    /// <summary>
    /// Accesses the registry again from the isolated plugin's code.
    /// </summary>
    public object CreateAgain() => PluginContext.CreateInstanceInReusedContext(typeof(FirstPlugin));
}