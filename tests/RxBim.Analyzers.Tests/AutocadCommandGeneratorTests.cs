namespace RxBim.Analyzers.Tests
{
    using System;
    using System.Linq;
    using Command.Generators.Autocad;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Verifies the command name and flags emitted by the AutoCAD generator without loading AutoCAD.
    /// </summary>
    public class AutocadCommandGeneratorTests
    {
        /// <summary>
        /// Reads the constructor argument semantically and keeps UI text out of command registration.
        /// </summary>
        /// <param name="arguments">The source attribute arguments.</param>
        /// <param name="expectedName">The expected generated CommandMethod name.</param>
        /// <param name="expectedFlags">The expected generated CommandMethod flags.</param>
        [Theory]
        [InlineData("\"GpRenameSymbols\", Text = \"Button text\"", "GpRenameSymbols", 0L)]
        [InlineData("CommandName, Text = \"Button text\"", "GpRenameSymbols", 0L)]
        [InlineData("CommandNames.Name, Text = \"Button text\"", "GpRenameSymbols", 0L)]
        [InlineData("nameof(TestCommand), Text = \"Button text\"", "TestCommand", 0L)]
        [InlineData("flags: CommandFlags.Session, commandName: CommandNames.Name, Text = \"Button text\"",
            "GpRenameSymbols", 1L)]
        [InlineData("CommandFlags.Session, Text = \"Button text\"", "TestCommand", 1L)]
        [InlineData("CommandName, Flags", "GpRenameSymbols", 3L)]
        [InlineData("CommandNames.Flags", "TestCommand", 3L)]
        [InlineData("CommandName, CommandFlags.Session | CommandFlags.UsePickSet", "GpRenameSymbols", 3L)]
        [InlineData("CommandName, (CommandFlags)3", "GpRenameSymbols", 3L)]
        [InlineData("CommandName, CommandFlags.Session & ~CommandFlags.Session", "GpRenameSymbols", 0L)]
        [InlineData("CommandName, CommandFlags.High | CommandFlags.Session", "GpRenameSymbols", 1099511627777L)]
        public void GeneratesCommandRegistrationFromConstructorArguments(
            string arguments, string expectedName, long expectedFlags)
        {
            var source = $$"""
                namespace Autodesk.AutoCAD.Runtime
                {
                    public enum CommandFlags : long { Modal = 0, Session = 1, UsePickSet = 2, High = 1L << 40 }
                    public class CommandMethodAttribute : System.Attribute
                    {
                        public CommandMethodAttribute(string name, CommandFlags flags) { }
                    }
                }
                namespace RxBim.Shared.Autocad
                {
                    using Autodesk.AutoCAD.Runtime;
                    public class RxBimCommandClassAttribute : System.Attribute
                    {
                        public RxBimCommandClassAttribute(CommandFlags flags) { }
                        public RxBimCommandClassAttribute(
                            string commandName, CommandFlags flags = CommandFlags.Modal) { }
                        public string Text { get; set; }
                    }
                }
                namespace RxBim.Command.Autocad
                {
                    public class RxBimCommand { public void Execute() { } }
                }
                namespace Example
                {
                    using Autodesk.AutoCAD.Runtime;
                    using RxBim.Command.Autocad;
                    using RxBim.Shared.Autocad;
                    public static class CommandNames
                    {
                        public const string Name = "GpRenameSymbols";
                        public const CommandFlags Flags = CommandFlags.Session | CommandFlags.UsePickSet;
                    }
                    [RxBimCommandClass({{arguments}})]
                    public class TestCommand : RxBimCommand
                    {
                        public const string CommandName = "GpRenameSymbols";
                        public const CommandFlags Flags = CommandFlags.Session | CommandFlags.UsePickSet;
                    }
                }
                """;
            var compilation = CSharpCompilation.Create("CommandGeneratorRegression",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            GeneratorDriver driver = CSharpGeneratorDriver.Create(new CommandGenerator());

            driver.RunGeneratorsAndUpdateCompilation(compilation, out var generated, out var diagnostics);

            var errors = diagnostics.Concat(generated.GetDiagnostics())
                .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToArray();

            errors.Length.Should().Be(0, string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
            var command = generated.GetTypeByMetadataName("Example.TestCommandGenerated")!
                .GetMembers("TestCommandExecute").OfType<IMethodSymbol>().Single();
            var registration = command.GetAttributes().Single(attribute =>
                attribute.AttributeClass?.Name == "CommandMethodAttribute");
            registration.ConstructorArguments[0].Value.Should().Be(expectedName);
            registration.ConstructorArguments[1].Value.Should().Be(expectedFlags);

        }
    }
}