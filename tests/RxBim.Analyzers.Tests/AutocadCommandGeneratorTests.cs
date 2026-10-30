namespace RxBim.Analyzers.Tests
{
    using System;
    using System.Linq;
    using Command.Generators.Autocad;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Verifies the command name and flags emitted by the AutoCAD generator without loading AutoCAD.
    /// </summary>
    [TestClass]
    public class AutocadCommandGeneratorTests
    {
        /// <summary>
        /// Reads the constructor argument semantically and keeps UI text out of command registration.
        /// </summary>
        /// <param name="arguments">The source attribute arguments.</param>
        /// <param name="expectedName">The expected generated CommandMethod name.</param>
        /// <param name="expectedFlags">The expected generated CommandMethod flags.</param>
        [DataTestMethod]
        [DataRow("\"GpRenameSymbols\", Text = \"Button text\"", "GpRenameSymbols", 0L)]
        [DataRow("CommandName, Text = \"Button text\"", "GpRenameSymbols", 0L)]
        [DataRow("CommandNames.Name, Text = \"Button text\"", "GpRenameSymbols", 0L)]
        [DataRow("nameof(TestCommand), Text = \"Button text\"", "TestCommand", 0L)]
        [DataRow("flags: CommandFlags.Session, commandName: CommandNames.Name, Text = \"Button text\"",
            "GpRenameSymbols", 1L)]
        [DataRow("CommandFlags.Session, Text = \"Button text\"", "TestCommand", 1L)]
        [DataRow("CommandName, Flags", "GpRenameSymbols", 3L)]
        [DataRow("CommandNames.Flags", "TestCommand", 3L)]
        [DataRow("CommandName, CommandFlags.Session | CommandFlags.UsePickSet", "GpRenameSymbols", 3L)]
        [DataRow("CommandName, (CommandFlags)3", "GpRenameSymbols", 3L)]
        [DataRow("CommandName, CommandFlags.Session & ~CommandFlags.Session", "GpRenameSymbols", 0L)]
        [DataRow("CommandName, CommandFlags.High | CommandFlags.Session", "GpRenameSymbols", 1099511627777L)]
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
            Assert.AreEqual(0, errors.Length,
                string.Join(Environment.NewLine, errors.Select(error => error.ToString())));
            var command = generated.GetTypeByMetadataName("Example.TestCommandGenerated")!
                .GetMembers("TestCommandExecute").OfType<IMethodSymbol>().Single();
            var registration = command.GetAttributes().Single(attribute =>
                attribute.AttributeClass?.Name == "CommandMethodAttribute");
            Assert.AreEqual(expectedName, registration.ConstructorArguments[0].Value);
            Assert.AreEqual(expectedFlags, registration.ConstructorArguments[1].Value);
        }
    }
}