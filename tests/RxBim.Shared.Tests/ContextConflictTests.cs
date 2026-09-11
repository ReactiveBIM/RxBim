namespace RxBim.Shared.Tests;

using System;
using System.Reflection;
using Xunit;
using static TestPluginDirectory;

public class ContextConflictTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ModelRetainedFromPreviousCommandCanOnlyBeCastInTheSameContext(bool reuse)
    {
        using var directory = new TestPluginDirectory();
        var type = directory.LoadType(FirstAssemblyName, "WpfPlugin");
        var first = Create(type, reuse);
        var second = Create(type, reuse);
        var firstModelType = ReadProperty<Type>(first, "ModelType");
        var secondModelType = ReadProperty<Type>(second, "ModelType");
        var retainedModel = Activator.CreateInstance(firstModelType)!;
        var cast = second.GetType().GetMethod("CastModel")!;

        // Identical names do not imply compatible runtime types.
        Assert.Equal(firstModelType.AssemblyQualifiedName, secondModelType.AssemblyQualifiedName);
        Assert.Equal(reuse, secondModelType.IsInstanceOfType(retainedModel));

        if (reuse)
        {
            Assert.Same(retainedModel, cast.Invoke(second, [retainedModel]));
        }
        else
        {
            var exception = Assert.Throws<TargetInvocationException>(() => cast.Invoke(second, [retainedModel]));
            Assert.IsType<InvalidCastException>(exception.InnerException);
        }
    }

    private static object Create(Type type, bool reuse) => reuse
        ? PluginContext.CreateInstanceInReusedContext(type)
        : PluginContext.CreateInstanceInNewContext(type)!;
}