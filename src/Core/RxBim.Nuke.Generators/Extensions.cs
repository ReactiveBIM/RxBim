namespace RxBim.Nuke.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns true if the symbol is in context assembly. Otherwise, returns false.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="symbol">The symbol to check.</param>
        public static bool CheckAssembly(this GeneratorExecutionContext context, ISymbol symbol)
        {
            return context.Compilation.Assembly.Equals(symbol.ContainingAssembly, SymbolEqualityComparer.Default);
        }

        /// <summary>
        /// Gets applications versions.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="versionNumbers">Applications versions.</param>
        /// <param name="inContextAssemblyOnly">
        /// True if the app version source location is allowed only in the context assembly. Otherwise, false.
        /// </param>
        /// <returns>True if application versions are collected successfully. Otherwise, false.</returns>
        public static bool TryGetVersionNumbers(
            this GeneratorExecutionContext context,
            out IReadOnlyCollection<string> versionNumbers,
            bool inContextAssemblyOnly)
        {
            var appVersion = context.Compilation.GetTypeByMetadataName("RxBim.Nuke.Versions.AppVersion");
            if (appVersion is null || (inContextAssemblyOnly && !context.CheckAssembly(appVersion)))
            {
                versionNumbers = null!;
                return false;
            }

            versionNumbers = GetVersionNumbers(appVersion);
            return true;
        }

        private static IReadOnlyCollection<string> GetVersionNumbers(INamespaceOrTypeSymbol appVersion)
        {
            var appVersionValues = appVersion.GetMembers()
                .Where(x => x.IsStatic && x.Kind is SymbolKind.Field)
                .Cast<IFieldSymbol>();

            var numbers = new HashSet<string>();

            foreach (var appVersionValue in appVersionValues)
            {
                if (TryGetVersionNumber(appVersionValue, out var number))
                    numbers.Add(number);
            }

            return numbers;
        }

        private static bool TryGetVersionNumber(ISymbol fieldSymbol, out string verNumber)
        {
            verNumber = string.Empty;

            var sourceSpan = fieldSymbol.Locations.First().SourceSpan;
            var syntaxReference = fieldSymbol.DeclaringSyntaxReferences.FirstOrDefault(x =>
                x.Span.Start <= sourceSpan.Start && x.Span.End >= sourceSpan.End);

            if (syntaxReference?.GetSyntax().FindNode(sourceSpan) is not VariableDeclaratorSyntax
                fieldDeclarationSyntax)
                return false;

            var appVerArgCreation = fieldDeclarationSyntax.DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .FirstOrDefault(x => x.Type is IdentifierNameSyntax { Identifier: { Text: "ApplicationVersion" } });

            var arg = appVerArgCreation?.ArgumentList?.Arguments.FirstOrDefault();
            if (arg?.Expression is not LiteralExpressionSyntax expressionSyntax)
                return false;

            verNumber = expressionSyntax.Token.ValueText;

            return !string.IsNullOrEmpty(verNumber);
        }
    }
}