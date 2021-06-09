namespace PikTools.Command.Autocad.Generators
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
            foreach (var (ns, command, flags) in commands)
            {
                AddCommand(ns, command, flags);
            }
        }

        private void AddCommand(
            string space,
            string command,
            string flags)
        {
            if (IsNullOrWhiteSpace(space))
            {
                throw new ArgumentNullException(nameof(space));
            }

            if (IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (IsNullOrWhiteSpace(flags))
            {
                flags = DefaultCommandFlag;
            }

            var classSource = Assembly.GetExecutingAssembly().ReadTextFromResource(CommandClassSource);

            classSource = classSource
                .Replace(Variables.Namespace, space)
                .Replace(Variables.Class, command)
                .Replace(Variables.Flags, flags)
                .Replace(Variables.Generated, Generated);

            _context.AddSource($"{command}{Generated}", classSource);
        }

        private List<(string Namespace, string Command, string CommandFlags)> GetCommands(
            INamedTypeSymbol attributeSymbol)
        {
            return _context.Compilation.SyntaxTrees.SelectMany(tree => tree.GetRoot().DescendantNodes())
                .OfType<ClassDeclarationSyntax>()
                .Where(
                    declarationSyntax => declarationSyntax.BaseList != null && declarationSyntax.BaseList.Types.Any(
                        baseTypeSyntax => baseTypeSyntax.Type is IdentifierNameSyntax identifier &&
                                          identifier.Identifier.Text == BaseCommandClassName))
                .Select(
                    s => (((NamespaceDeclarationSyntax)s.Parent)?.Name.ToString(), s.Identifier.Text,
                        GetFlags(s, attributeSymbol)))
                .ToList();
        }

        private string GetFlags(ClassDeclarationSyntax declaredClass, INamedTypeSymbol attributeSymbol)
        {
            if (attributeSymbol == null)
            {
                return Empty;
            }

            var semanticModel = _context.Compilation.GetSemanticModel(declaredClass.SyntaxTree);

            var nodes = declaredClass
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

            return nodes.ReadCommandFlags();
        }
    }
}