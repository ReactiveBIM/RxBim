namespace RxBim.Shared.Tests.Second;

using System;
using Dependency;

/// <summary>
/// The second entry point of the same application, located in a separate DLL.
/// </summary>
public sealed class SecondPlugin
{
    /// <summary>
    /// The object's sequence number in the application's shared state.
    /// </summary>
    public int Sequence { get; } = Counter.Next();

    /// <summary>
    /// A type from the application's private dependency.
    /// </summary>
    public Type DependencyType => typeof(Counter);
}