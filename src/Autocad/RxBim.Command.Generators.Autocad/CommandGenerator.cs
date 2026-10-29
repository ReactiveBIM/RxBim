namespace RxBim.Command.Generators.Autocad
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static System.String;
    using static Constants;

    /// <summary>
    /// Code generator for Autocad command class.
    /// </summary>
    [Generator]
    public class CommandGenerator : ISourceGenerator
    {
        private GeneratorExecutionContext _context;

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
             // Debugger.Launch();
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            _context = context;
            var attributeSymbol = GetAttribute();
            AddCommands(attributeSymbol);
        }

        private static object? ReadConstructorArgument(AttributeData? attribute, string parameterName)
        {
            if (attribute?.AttributeConstructor is null)
                return null;

            // Roslyn orders arguments by constructor parameters and evaluates defaults, constants and enum expressions.
            var parameters = attribute.AttributeConstructor.Parameters;

            for (var index = 0; index < parameters.Length; index++)
            {
                if (parameters[index].Name == parameterName)
                    return attribute.ConstructorArguments[index].Value;
            }

            return null;
        }

        private INamedTypeSymbol GetAttribute()
        {
            return _context.Compilation.GetTypeByMetadataName(CommandClassAttributeTypeFullName)!;
        }

        private void AddCommands(INamedTypeSymbol? attributeSymbol)
        {
            var commands = GetCommands(attributeSymbol);
            foreach (var (ns, commandClass, commandName, flags) in commands)
            {
                AddCommand(ns, commandClass, commandName, flags);
            }
        }

        private void AddCommand(
            string space,
            string commandClass,
            string commandName,
            string flags)
        {
            if (IsNullOrWhiteSpace(space))
            {
                throw new ArgumentNullException(nameof(space));
            }

            if (IsNullOrWhiteSpace(commandClass))
            {
                throw new ArgumentNullException(nameof(commandClass));
            }

            if (IsNullOrWhiteSpace(commandName))
            {
                commandName = commandClass;
            }

            if (IsNullOrWhiteSpace(flags))
            {
                flags = DefaultCommandFlag;
            }

            var classSource = Assembly.GetExecutingAssembly().ReadTextFromResource(CommandClassSource);

            classSource = classSource
                .Replace(Variables.Namespace, space)
                .Replace(Variables.Class, commandClass)
                .Replace(Variables.Flags, flags)
                .Replace(Variables.Generated, Generated)
                .Replace(Variables.CommandName, commandName);

            _context.AddSource($"{commandClass}{Generated}", classSource);
        }

        private List<(string Namespace, string CommandClass, string CommandName, string CommandFlags)> GetCommands(
            ISymbol? attributeSymbol)
        {
            return _context.Compilation.SyntaxTrees.SelectMany(tree => tree.GetRoot().DescendantNodes())
                .OfType<ClassDeclarationSyntax>()
                .Where(CheckForCommandType)
                .Select(
                    s =>
                    {
                        var attribute = GetCommandAttribute(s, attributeSymbol);
                        var commandName = ReadConstructorArgument(attribute, CommandNameParameter) as string ?? Empty;
                        var flags = ReadConstructorArgument(attribute, CommandFlagsParameter);
                        var flagsLiteral = flags is null
                            ? Empty
                            : $"({CommandFlags}){Convert.ToString(flags, CultureInfo.InvariantCulture)}";

                        return (
                            ((BaseNamespaceDeclarationSyntax)s.Parent!).Name.ToString(),
                            s.Identifier.Text,
                            commandName,
                            flagsLiteral);
                    })
                .ToList();
        }

        private AttributeData? GetCommandAttribute(ClassDeclarationSyntax declaredClass, ISymbol? attributeSymbol)
        {
            var semanticModel = _context.Compilation.GetSemanticModel(declaredClass.SyntaxTree);
            var declaredSymbol = semanticModel.GetDeclaredSymbol(declaredClass);
            return declaredSymbol?.GetAttributes().FirstOrDefault(data =>
                SymbolEqualityComparer.Default.Equals(data.AttributeClass, attributeSymbol));
        }

        private bool CheckForCommandType(BaseTypeDeclarationSyntax typeDeclarationSyntax)
        {
            var semanticModel = _context.Compilation.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
            var declaredSymbol = semanticModel.GetDeclaredSymbol(typeDeclarationSyntax);

            while (declaredSymbol != null)
            {
                if (declaredSymbol.Name == BaseCommandClassName)
                    return true;

                declaredSymbol = declaredSymbol.BaseType;
            }

            return false;
        }
    }
}