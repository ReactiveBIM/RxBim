namespace RxBim.Application.Ribbon.Tests;

using System;
using System.Runtime.Loader;
using FluentAssertions;
using Xunit;

public class AssemblyExtensionsTests
{
    [Fact]
    public void GetTypeByNamePreservesCommandTypeAndAttributesInMenuContext()
    {
        var context = new AssemblyLoadContext("RibbonTests", isCollectible: true);

        try
        {
            var menuAssembly = context.LoadFromAssemblyPath(typeof(MenuData).Assembly.Location);
            var commandAssembly = context.LoadFromAssemblyPath(typeof(AttributedCommand).Assembly.Location);
            var expected = commandAssembly.GetType(typeof(AttributedCommand).FullName!)!;
            var attributeType = commandAssembly.GetType(typeof(CommandMetadataAttribute).FullName!)!;

            var actual = menuAssembly.GetTypeByName(expected.AssemblyQualifiedName!);

            actual.Should().BeSameAs(expected);
            AssemblyLoadContext.GetLoadContext(actual.Assembly).Should().BeSameAs(context);
            actual.GetCustomAttributes(attributeType, inherit: true).Should().ContainSingle();
        }
        finally
        {
            context.Unload();
        }
    }

    [Fact]
    public void GetTypeByNameKeepsDefaultContextBehavior()
    {
        var actual = typeof(MenuData).Assembly.GetTypeByName(typeof(AttributedCommand).AssemblyQualifiedName!);

        actual.Should().BeSameAs(typeof(AttributedCommand));
    }

    [CommandMetadata]
    public class AttributedCommand
    {
    }

    public sealed class CommandMetadataAttribute : Attribute
    {
    }
}