namespace RxBim.Shared.Tests.WpfProbe;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            Run(args[0], args[1], bool.Parse(args[2]));
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void Run(string firstDirectory, string secondDirectory, bool reuse)
    {
        const string entryName = "RxBim.Shared.Tests.First";
        const string dependencyName = "RxBim.Shared.Tests.Dependency";
        var source = new AssemblyLoadContext("Source metadata", isCollectible: true);
        var type = source.LoadFromAssemblyPath(Path.Combine(firstDirectory, entryName + ".dll"))
            .GetType(entryName + ".WpfPlugin", throwOnError: true)!;
        var first = Create(type, reuse);
        var secondSource = new AssemblyLoadContext("Second source metadata", isCollectible: true);
        var secondType = secondSource.LoadFromAssemblyPath(Path.Combine(secondDirectory, entryName + ".dll"))
            .GetType(type.FullName!, throwOnError: true)!;
        var second = Create(secondType, reuse);
        var firstModel = (Type)ReadProperty(first, "ModelType");
        var secondModel = (Type)ReadProperty(second, "ModelType");

        // Model the global host resolver in the linked reproduction: XAML requests
        // without an addin requester receive the first loaded private dependency.
        // https://github.com/cunhamauro/XamlParserTypeIdentityMismatch
        AppDomain.CurrentDomain.AssemblyResolve += (_, request) =>
            new AssemblyName(request.Name).Name == dependencyName ? firstModel.Assembly : null;

        var firstResult = (object[])first.GetType().GetMethod("ReadTemplate")!.Invoke(first, null)!;
        var secondResult = (object[])second.GetType().GetMethod("ReadTemplate")!.Invoke(second, null)!;
        Console.WriteLine(JsonSerializer.Serialize(new
        {
            SameModelType = firstModel == secondModel,
            FirstTemplateMatches = (bool)firstResult[1],
            SecondTemplateMatches = (bool)secondResult[1],
            SecondTemplateUsesFirstModel = (Type)secondResult[0] == firstModel,
            SecondTemplateUsesSecondModel = (Type)secondResult[0] == secondModel
        }));
    }

    private static object Create(Type type, bool reuse) => reuse
        ? PluginContext.CreateInstanceInReusedContext(type)
        : PluginContext.CreateInstanceInNewContext(type)
          ?? throw new InvalidOperationException("Could not create the isolated plugin.");

    private static object ReadProperty(object instance, string name) =>
        instance.GetType().GetProperty(name)!.GetValue(instance)!;
}