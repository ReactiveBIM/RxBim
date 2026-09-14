namespace RxBim.Shared.Tests;

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class WpfContextConflictTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task XamlTemplateMatchesAcrossCommandsOnlyWhenTheContextIsShared(bool reuse, bool differentDirectories)
    {
        using var first = new TestPluginDirectory();
        using var second = new TestPluginDirectory();
        var start = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        start.ArgumentList.Add(Path.Combine(AppContext.BaseDirectory, "WpfProbe", "RxBim.Shared.Tests.WpfProbe.dll"));
        start.ArgumentList.Add(first.DirectoryPath);
        start.ArgumentList.Add(differentDirectories ? second.DirectoryPath : first.DirectoryPath);
        start.ArgumentList.Add(reuse.ToString());

        // WPF caches assembly/type lookups for the process lifetime. Each case needs a fresh process.
        using var process = Process.Start(start)!;
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));

        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        finally
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
        }

        var output = await outputTask;
        var error = await errorTask;
        Assert.True(process.ExitCode == 0, $"WPF probe exited with {process.ExitCode}.\n{output}\n{error}");
        using var result = JsonDocument.Parse(output);
        var expectedMatch = reuse && !differentDirectories;
        Assert.True(result.RootElement.GetProperty("FirstTemplateMatches").GetBoolean());
        Assert.True(result.RootElement.GetProperty("SecondTemplateUsesFirstModel").GetBoolean());
        Assert.Equal(expectedMatch, result.RootElement.GetProperty("SameModelType").GetBoolean());
        Assert.Equal(expectedMatch, result.RootElement.GetProperty("SecondTemplateMatches").GetBoolean());
        Assert.Equal(expectedMatch, result.RootElement.GetProperty("SecondTemplateUsesSecondModel").GetBoolean());
    }
}