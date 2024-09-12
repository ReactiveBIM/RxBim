#if NETCOREAPP
namespace RxBim.Shared;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

/// <inheritdoc />
public class PluginContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="assemblyPath">Assembly path.</param>
    /// <param name="pluginName">Plugin name</param>
    public PluginContext(string assemblyPath, string pluginName)
     : base(pluginName)
    {
        _resolver = new AssemblyDependencyResolver(assemblyPath);
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
    /// Creates instance of specified type in separated context;
    /// </summary>
    /// <param name="type">Type.</param>
    public static object? CreateInstance(Type type)
    {
        var assembly = type.Assembly;
        var location = assembly.Location;
        var pluginName = Path.GetFileName(Path.GetDirectoryName(location)!);
        var context = new PluginContext(location, pluginName);
        var loadedAssembly = context.LoadFromAssemblyPath(location);
        return loadedAssembly.CreateInstance(type.FullName!);
    }

    /// <inheritdoc />
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        return assemblyPath is not null ? LoadFromAssemblyPath(assemblyPath) : null;
    }
}
#endif