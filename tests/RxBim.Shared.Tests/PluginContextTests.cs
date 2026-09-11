namespace RxBim.Shared.Tests;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Xunit;
using static TestPluginDirectory;

public class PluginContextTests
{
    [Fact]
    public void NewContextCreatesDifferentTypesAndIndependentState()
    {
        using var directory = new TestPluginDirectory();
        var first = PluginContext.CreateInstanceInNewContext(directory.FirstType)!;
        var second = PluginContext.CreateInstanceInNewContext(directory.FirstType)!;

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.NotSame(ContextOf(first), ContextOf(second));
        Assert.NotEqual(first.GetType(), second.GetType());
        Assert.Equal(1, ReadProperty<int>(first, "Sequence"));
        Assert.Equal(1, ReadProperty<int>(second, "Sequence"));
    }

    [Fact]
    public void ReusedContextCreatesNewObjectsWithTheSameTypeAndSharedState()
    {
        using var directory = new TestPluginDirectory();
        var first = PluginContext.CreateInstanceInReusedContext(directory.FirstType);
        var second = PluginContext.CreateInstanceInReusedContext(directory.FirstType);

        Assert.NotSame(first, second);
        Assert.Same(ContextOf(first), ContextOf(second));
        Assert.Equal(first.GetType(), second.GetType());
        Assert.Equal(1, ReadProperty<int>(first, "Sequence"));
        Assert.Equal(2, ReadProperty<int>(second, "Sequence"));
        Assert.False(ContextOf(first).IsCollectible);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DifferentAssembliesShareTheDirectoryContextInEitherOrder(bool reverse)
    {
        using var directory = new TestPluginDirectory();
        var first = PluginContext.CreateInstanceInReusedContext(reverse ? directory.SecondType : directory.FirstType);
        var second = PluginContext.CreateInstanceInReusedContext(reverse ? directory.FirstType : directory.SecondType);

        Assert.Same(ContextOf(first), ContextOf(second));
        Assert.NotSame(first.GetType().Assembly, second.GetType().Assembly);
        Assert.Same(ReadProperty<Type>(first, "DependencyType"), ReadProperty<Type>(second, "DependencyType"));
        Assert.Equal(1, ReadProperty<int>(first, "Sequence"));
        Assert.Equal(2, ReadProperty<int>(second, "Sequence"));
    }

    [Fact]
    public void DifferentDirectoriesRemainIsolated()
    {
        using var firstDirectory = new TestPluginDirectory();
        using var secondDirectory = new TestPluginDirectory();
        var first = PluginContext.CreateInstanceInReusedContext(firstDirectory.FirstType);
        var second = PluginContext.CreateInstanceInReusedContext(secondDirectory.FirstType);

        Assert.NotSame(ContextOf(first), ContextOf(second));
        Assert.NotEqual(first.GetType(), second.GetType());
        Assert.Equal(1, ReadProperty<int>(second, "Sequence"));
    }

    [Fact]
    public void InfrastructureAndRegistryAreSharedWithTheIsolatedPlugin()
    {
        using var directory = new TestPluginDirectory();
        var first = PluginContext.CreateInstanceInReusedContext(directory.FirstType);
        var second = first.GetType().GetMethod("CreateAgain")!.Invoke(first, null)!;

        Assert.Same(typeof(PluginContext), ReadProperty<Type>(first, "InfrastructureType"));
        Assert.Same(ContextOf(first), ContextOf(second));
        Assert.Equal(2, ReadProperty<int>(second, "Sequence"));
    }

    [Fact]
    public void ConstructorFailureDoesNotDiscardTheContext()
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "FailOncePlugin");
        var exception = Assert.Throws<TargetInvocationException>(() => PluginContext.CreateInstanceInReusedContext(type));
        Assert.IsType<InvalidOperationException>(exception.InnerException);

        var recovered = PluginContext.CreateInstanceInReusedContext(type);
        var other = PluginContext.CreateInstanceInReusedContext(directory.FirstType);
        Assert.Same(ContextOf(other), ContextOf(recovered));
    }

    [Fact]
    public void LegacyMethodsStillReturnNullOnActivationFailure()
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "FailOncePlugin");
        Assert.Null(PluginContext.CreateInstanceInNewContext(type));
        var context = new PluginContext(type.Assembly.Location, "Legacy test");
        Assert.Null(context.CreateInstanceInContext(type));
        Assert.NotNull(context.CreateInstanceInContext(type));
    }

    [Fact]
    public void ReusedAndNewModesDoNotReuseEachOthersContext()
    {
        using var directory = new TestPluginDirectory();
        var reused = PluginContext.CreateInstanceInReusedContext(directory.FirstType);
        var fresh = PluginContext.CreateInstanceInNewContext(directory.FirstType)!;
        Assert.NotSame(ContextOf(reused), ContextOf(fresh));
    }

    [Fact]
    public void ReusedContextLoadsDependenciesWithoutManifests()
    {
        using var directory = new TestPluginDirectory();

        foreach (var manifest in Directory.EnumerateFiles(directory.DirectoryPath, "*.deps.json"))
            File.Delete(manifest);

        var instance = PluginContext.CreateInstanceInReusedContext(directory.SecondType);
        var dependency = ReadProperty<Type>(instance, "DependencyType");
        Assert.Same(ContextOf(instance), AssemblyLoadContext.GetLoadContext(dependency.Assembly));
    }

    [Fact]
    public void ReusedContextDoesNotInspectUnrelatedManifestsOrDlls()
    {
        using var directory = new TestPluginDirectory();

        File.WriteAllText(Path.Combine(directory.DirectoryPath, "Unrelated.deps.json"), "Invalid JSON");
        File.WriteAllText(Path.Combine(directory.DirectoryPath, "Unrelated.dll"), "Not an assembly");
        var first = PluginContext.CreateInstanceInReusedContext(directory.FirstType);
        var second = PluginContext.CreateInstanceInReusedContext(directory.SecondType);

        Assert.Same(ContextOf(first), ContextOf(second));
        Assert.Equal(1, ReadProperty<int>(first, "Sequence"));
        Assert.Equal(2, ReadProperty<int>(second, "Sequence"));
    }

    [Fact]
    public void ReusedContextLoadsAssemblyNotListedInTheFirstManifest()
    {
        using var directory = new TestPluginDirectory();
        var firstType = directory.FirstType;
        var requestedName = directory.SecondType.Assembly.GetName();
        var resolver = new AssemblyDependencyResolver(firstType.Assembly.Location);
        Assert.Null(resolver.ResolveAssemblyToPath(requestedName));

        var instance = PluginContext.CreateInstanceInReusedContext(firstType);
        var context = ContextOf(instance);
        var assembly = context.LoadFromAssemblyName(requestedName);

        Assert.Equal(Path.Combine(directory.DirectoryPath, SecondAssemblyName + ".dll"), assembly.Location);
        Assert.Same(context, AssemblyLoadContext.GetLoadContext(assembly));
    }

    [Fact]
    public void DirectoryKeyIgnoresPathCaseAndSegments()
    {
        using var directory = new TestPluginDirectory();
        var first = PluginContext.CreateInstanceInReusedContext(directory.FirstType);
        var source = new AssemblyLoadContext("Case variant", isCollectible: true);

        try
        {
            var path = Path.Combine(directory.DirectoryPath, ".", FirstAssemblyName + ".dll").ToUpperInvariant();
            var type = source.LoadFromAssemblyPath(path).GetType(FirstAssemblyName + ".FirstPlugin")!;
            var second = PluginContext.CreateInstanceInReusedContext(type);
            Assert.Same(ContextOf(first), ContextOf(second));
        }
        finally
        {
            source.Unload();
        }
    }

    [Fact]
    public void MissingDependencyIsReportedWithoutFallingBackToSourceContext()
    {
        using var directory = new TestPluginDirectory();
        File.Delete(Path.Combine(directory.DirectoryPath, DependencyAssemblyName + ".dll"));
        var exception = Assert.ThrowsAny<Exception>(() => PluginContext.CreateInstanceInReusedContext(directory.SecondType));
        Assert.IsType<FileNotFoundException>(exception.GetBaseException());
    }

    [Fact]
    public void NewContextStillResolvesPrivateAssetsFromManifest()
    {
        using var directory = new TestPluginDirectory();
        var privateDirectory = Path.Combine(directory.DirectoryPath, "private");
        Directory.CreateDirectory(privateDirectory);
        var dependencyPath = Path.Combine(privateDirectory, DependencyAssemblyName + ".dll");
        File.Move(Path.Combine(directory.DirectoryPath, DependencyAssemblyName + ".dll"), dependencyPath);
        directory.SetDependencyAsset(SecondAssemblyName, "private/" + DependencyAssemblyName + ".dll");

        var instance = PluginContext.CreateInstanceInNewContext(directory.SecondType);

        Assert.NotNull(instance);
        Assert.Equal(dependencyPath, ReadProperty<Type>(instance!, "DependencyType").Assembly.Location);
    }

    [Fact]
    public void ReusedContextPrefersManifestPathToFileInDirectory()
    {
        using var directory = new TestPluginDirectory();
        var privateDirectory = Path.Combine(directory.DirectoryPath, "private");
        Directory.CreateDirectory(privateDirectory);
        var dependencyPath = Path.Combine(directory.DirectoryPath, DependencyAssemblyName + ".dll");
        File.Copy(dependencyPath, Path.Combine(privateDirectory, DependencyAssemblyName + ".dll"));
        directory.SetDependencyAsset(SecondAssemblyName, "private/" + DependencyAssemblyName + ".dll");

        var first = PluginContext.CreateInstanceInReusedContext(directory.SecondType);
        var second = PluginContext.CreateInstanceInReusedContext(directory.FirstType);

        Assert.Equal(Path.Combine(privateDirectory, DependencyAssemblyName + ".dll"),
            ReadProperty<Type>(first, "DependencyType").Assembly.Location);
        Assert.Same(ReadProperty<Type>(first, "DependencyType"), ReadProperty<Type>(second, "DependencyType"));
    }

    [Fact]
    public void DependencyVersionResolutionMatchesLegacyContext()
    {
        using var directory = new TestPluginDirectory();
        var legacy = PluginContext.CreateInstanceInNewContext(directory.SecondType)!;
        var reused = PluginContext.CreateInstanceInReusedContext(directory.SecondType);
        var dependency = ReadProperty<Type>(reused, "DependencyType").Assembly;
        var dependencyName = dependency.GetName();
        dependencyName.Version = new Version(2, 0, 0, 0);

        var legacyAssembly = ContextOf(legacy).LoadFromAssemblyName(dependencyName);
        var reusedAssembly = ContextOf(reused).LoadFromAssemblyName(dependencyName);

        Assert.Equal(legacyAssembly.GetName().FullName, reusedAssembly.GetName().FullName);
        Assert.Same(dependency, reusedAssembly);
    }
}