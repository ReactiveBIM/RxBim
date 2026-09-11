#if NETCOREAPP
namespace RxBim.Shared;

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

/// <inheritdoc />
public class PluginContext : AssemblyLoadContext
{
    private const string ContextNamePrefix = "RxBim:";
    private const string AssemblyExtension = ".dll";
    private static readonly ConcurrentDictionary<string, Lazy<PluginContext>> ReusedContexts =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly AssemblyDependencyResolver _resolver;
    private readonly string? _directory;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="assemblyPath">Assembly path.</param>
    /// <param name="pluginName">Plugin name</param>
    public PluginContext(string assemblyPath, string pluginName)
     : base($"{ContextNamePrefix}{pluginName}")
    {
        _resolver = new AssemblyDependencyResolver(assemblyPath);
    }

    private PluginContext(string assemblyPath)
        : this(assemblyPath, Path.GetDirectoryName(assemblyPath)!)
    {
        // Reused contexts also search the directory for other commands' dependencies.
        _directory = Path.GetDirectoryName(assemblyPath);
    }

    /// <summary>
    /// Determines whether the type is the type not associated with the default context.
    /// </summary>
    /// <param name="type">Type.</param>
    public static bool IsCurrentContextDefault(Type type)
    {
        var currentContext = GetLoadContext(type.Assembly);
        return currentContext == Default;
    }

    /// <summary>
    /// Determines whether the type is loaded into a context managed by RxBim.
    /// </summary>
    /// <param name="type">Type.</param>
    public static bool IsCurrentContextRxBim(Type type)
    {
        var currentContext = GetLoadContext(type.Assembly);
        return currentContext?.Name?.StartsWith(ContextNamePrefix, StringComparison.Ordinal) == true;
    }

    /// <summary>
    /// Creates instance of specified type in separated context;
    /// </summary>
    /// <param name="type">Type.</param>
    public static object? CreateInstanceInNewContext(Type type)
    {
        try
        {
            var assembly = type.Assembly;
            var location = assembly.Location;
            var pluginName = Path.GetFileName(location);
            var context = new PluginContext(location, pluginName);
            var loadedAssembly = context.LoadFromAssemblyPath(location);
            return loadedAssembly.CreateInstance(type.FullName!);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new object in the application directory's shared context, retained until the process exits.
    /// </summary>
    /// <param name="type">The type to instantiate from an application assembly.</param>
    /// <remarks>
    /// Loading and constructor errors propagate to the caller without removing the context from the registry.
    /// Dependencies are resolved using the first assembly's manifest, then the application directory.
    /// Application assemblies must reside in the same directory.
    /// Updating DLLs requires restarting the host.
    /// </remarks>
    public static object CreateInstanceInReusedContext(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var assemblyPath = type.Assembly.Location;

        if (string.IsNullOrEmpty(assemblyPath))
            throw new ArgumentException("A reused context requires an assembly with a DLL path.", nameof(type));

        assemblyPath = Path.GetFullPath(assemblyPath);
        var directory = Path.TrimEndingDirectorySeparator(Path.GetDirectoryName(assemblyPath)!);

        // Lazy prevents concurrent calls from creating redundant non-collectible contexts.
        var context = ReusedContexts.GetOrAdd(directory, _ => new Lazy<PluginContext>(
            () => new PluginContext(assemblyPath),
            LazyThreadSafetyMode.ExecutionAndPublication)).Value;

        return context.CreateInstanceCore(type);
    }

    /// <summary>
    /// Creates instance of specified type in current context;
    /// </summary>
    /// <param name="type">Type.</param>
    public object? CreateInstanceInContext(Type type)
    {
        try
        {
            return CreateInstanceCore(type);
        }
        catch
        {
            return null;
        }
    }

    /// <inheritdoc />
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        if (assemblyPath is null && _directory is not null)
            assemblyPath = GetLocalAssemblyPath(assemblyName);

        if (assemblyPath is null)
            return null;

        if (AssemblyMetadataReader.HasAttribute(assemblyPath, nameof(SharedLibraryAttribute)))
        {
            var alreadyInDefault = Default.Assemblies
                .Any(a => a.GetName().FullName == assemblyName.FullName);
            return alreadyInDefault ? null : Default.LoadFromAssemblyPath(assemblyPath);
        }

        return LoadFromAssemblyPath(assemblyPath);
    }

    private string? GetLocalAssemblyPath(AssemblyName assemblyName)
    {
        var name = assemblyName.Name;
        var culture = assemblyName.CultureName;

        if (string.IsNullOrEmpty(name) || Path.GetFileName(name) != name
            || (!string.IsNullOrEmpty(culture) && Path.GetFileName(culture) != culture))
            return null;

        var directory = string.IsNullOrEmpty(culture) ? _directory! : Path.Combine(_directory!, culture);
        var path = Path.Combine(directory, name + AssemblyExtension);

        return File.Exists(path) ? path : null;
    }

    private object CreateInstanceCore(Type type)
    {
        var loadedAssembly = LoadFromAssemblyPath(type.Assembly.Location);
        return loadedAssembly.CreateInstance(type.FullName!)
               ?? throw new TypeLoadException($"Could not instantiate type '{type.FullName}' from '{loadedAssembly.Location}'.");
    }
}
#endif