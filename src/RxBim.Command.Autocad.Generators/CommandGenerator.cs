namespace RxBim.Command.Autocad.Generators
{
    using System;
    using System.Collections.Generic;
    //// using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using static System.String;
    using static Constants;

    /// <summary>
    /// Генератор кода для командного класса
    /// </summary>
    [Generator]
    public class CommandGenerator : ISourceGenerator
    {
        private GeneratorExecutionContext _context;

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
// #if DEBUG
//             // Для отладки генератора
//             Debugger.Launch();
// #endif
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            _context = context;
            var attributeSymbol = GetAttribute();
            AddCommands(attributeSymbol);
        }

        private INamedTypeSymbol GetAttribute()
        {
            return _context.Compilation.GetTypeByMetadataName(CommandClassAttributeTypeFullName);
        }

        private void AddCommands(INamedTypeSymbol attributeSymbol)
        {
            var commands = GetCommands(attributeSymbol);
            foreach (var (ns, command, commandName, flags) in commands)
            {
                AddCommand(ns, command, commandName, flags);
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

        private List<(string Namespace, string Command, string commandName, string CommandFlags)> GetCommands(
            INamedTypeSymbol attributeSymbol)
        {
            return _context.Compilation.SyntaxTrees.SelectMany(tree => tree.GetRoot().DescendantNodes())
                .OfType<ClassDeclarationSyntax>()
                .Where(
                    declarationSyntax => declarationSyntax.BaseList != null && declarationSyntax.BaseList.Types.Any(
                        baseTypeSyntax =>
                            baseTypeSyntax.Type is IdentifierNameSyntax { Identifier: { Text: BaseCommandClassName } }))
                .Select(
                    s =>
                    {
                        var tokens = GetAttributeTokens(s, attributeSymbol);
                        return (((NamespaceDeclarationSyntax)s.Parent)?.Name.ToString(), s.Identifier.Text,
                            tokens.ReadCommandName(), tokens.ReadCommandFlags());
                    })
                .ToList();
        }

        private List<SyntaxToken> GetAttributeTokens(SyntaxNode declaredClass, ISymbol attributeSymbol)
        {
            if (attributeSymbol == null)
            {
                return new List<SyntaxToken>();
            }

            var semanticModel = _context.Compilation.GetSemanticModel(declaredClass.SyntaxTree);

            return declaredClass
                .DescendantNodes()
                .OfType<AttributeSyntax>()
                .FirstOrDefault(
                    a => a.DescendantTokens()
                        .Any(
                            dt =>
                                dt.IsKind(SyntaxKind.IdentifierToken) &&
                                dt.Parent != null &&
                                semanticModel.GetTypeInfo(dt.Parent).Type?.Name == attributeSymbol.Name))
                ?.DescendantTokens()
                .ToList();
        }
    }
}