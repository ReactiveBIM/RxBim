namespace RxBim.Shared.Tests.First;

using System;
using System.Threading;

/// <summary>
/// Verifies that the context is retained after a constructor failure.
/// </summary>
public sealed class FailOncePlugin
{
    private static int _attempts;

    /// <summary>
    /// Fails on the first constructor call and creates an object on subsequent calls.
    /// </summary>
    public FailOncePlugin()
    {
        if (Interlocked.Increment(ref _attempts) == 1)
            throw new InvalidOperationException("The first constructor call failed.");
    }
}