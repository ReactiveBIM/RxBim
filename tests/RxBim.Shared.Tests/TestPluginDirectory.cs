namespace RxBim.Shared.Tests;

using System;
using System.IO;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json.Nodes;

internal sealed class TestPluginDirectory : IDisposable
{
    internal const string FirstAssemblyName = "RxBim.Shared.Tests.First";
    internal const string SecondAssemblyName = "RxBim.Shared.Tests.Second";
    internal const string DependencyAssemblyName = "RxBim.Shared.Tests.Dependency";
    private readonly AssemblyLoadContext _source = new("Fixture source", isCollectible: true);

    public TestPluginDirectory()
    {
        // Keep files under the ignored bin directory: non-collectible contexts may lock DLLs until testhost exits.
        DirectoryPath = Path.Combine(AppContext.BaseDirectory, "TestApplications", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(DirectoryPath);

        foreach (var file in Directory.EnumerateFiles(Path.Combine(AppContext.BaseDirectory, "Fixtures")))
            File.Copy(file, Path.Combine(DirectoryPath, Path.GetFileName(file)));
    }

    public string DirectoryPath { get; }

    public Type FirstType => LoadType(FirstAssemblyName, "FirstPlugin");

    public Type SecondType => LoadType(SecondAssemblyName, "SecondPlugin");

    public Type LoadType(string assemblyName, string className)
    {
        var assembly = _source.LoadFromAssemblyPath(Path.Combine(DirectoryPath, assemblyName + ".dll"));
        return assembly.GetType(assemblyName + "." + className, throwOnError: true)!;
    }

    public void SetDependencyAsset(string entryAssemblyName, string relativePath)
    {
        const string targetName = ".NETCoreApp,Version=v8.0";
        var manifestPath = Path.Combine(DirectoryPath, entryAssemblyName + ".deps.json");
        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        var dependency = manifest["targets"]![targetName]![DependencyAssemblyName + "/1.0.0"]!.AsObject();
        dependency.Remove("runtime");
        dependency["runtimeTargets"] = new JsonObject
        {
            [relativePath] = new JsonObject { ["rid"] = "win-x64", ["assetType"] = "runtime" }
        };
        File.WriteAllText(manifestPath, manifest.ToJsonString(), new UTF8Encoding(true));
    }

    public void Dispose() => _source.Unload();

    public static T ReadProperty<T>(object instance, string name) =>
        (T)instance.GetType().GetProperty(name)!.GetValue(instance)!;

    public static AssemblyLoadContext ContextOf(object instance) =>
        AssemblyLoadContext.GetLoadContext(instance.GetType().Assembly)!;
}