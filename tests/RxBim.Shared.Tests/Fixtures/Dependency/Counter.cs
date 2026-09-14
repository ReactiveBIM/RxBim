namespace RxBim.Shared.Tests.Dependency;

using System.Threading;

/// <summary>
/// Shared state for test plugins within a single context.
/// </summary>
public static class Counter
{
    private static int _value;

    /// <summary>
    /// Increments the count of created objects.
    /// </summary>
    public static int Next() => Interlocked.Increment(ref _value);
}