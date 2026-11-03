namespace RxBim.Nuke.Tests;

using System;
using System.IO;
using System.Linq;
using Builders;
using FluentAssertions;
using global::Nuke.Common.IO;
using InnoSetup.ScriptBuilder;
using Xunit;

public class InnoBuilderTests
{
    [Fact]
    public void ShouldPreventParallelSetupsOfProject()
    {
        var options = new BuildOptions
        {
            ProjectName = "FamilyManager",
            ProductProjectName = "Product title",
            Version = "1.0.0",
            InstallDir = "%AppDataFolder%/Plugins",
            OutFileName = "Installer",
            PackageGuid = "EB183861-E6AC-4E36-9213-F4F604B83C41"
        };
        var scriptPath = Path.GetTempFileName();

        try
        {
            InnoBuilder.Create(options).Build((AbsolutePath)scriptPath);

            var lines = File.ReadAllLines(scriptPath);
            GetValues(lines, "SetupMutex").Should().Equal("FamilyManager");
            GetValues(lines, "SetupAppRunningError").Should().ContainSingle()
                .Which.Should().StartWith("FamilyManager setup has detected that another instance is currently running.");
        }
        finally
        {
            File.Delete(scriptPath);
        }
    }

    private static string[] GetValues(string[] lines, string key) => lines
        .Where(line => line.StartsWith($"{key}=", StringComparison.Ordinal))
        .Select(line => line.Split('=', 2)[1].Trim())
        .ToArray();
}