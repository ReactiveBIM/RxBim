namespace RxBim.Nuke.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Extensions for <see cref="GeneratorExecutionContext"/>.
    /// </summary>
    internal static class GeneratorExecutionContextExtensions
    {
        /// <summary>
        /// Returns true if the symbol is in context assembly. Otherwise, returns false.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="symbol">The symbol to check.</param>
        public static bool IsSymbolInContext(this GeneratorExecutionContext context, ISymbol symbol)
        {
            return context.Compilation.Assembly.Equals(symbol.ContainingAssembly, SymbolEqualityComparer.Default);
        }

        /// <summary>
        /// Gets applications versions numbers.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="versionNumbers">Applications versions.</param>
        /// <returns>True if application versions are collected successfully. Otherwise, false.</returns>
        public static bool TryGetVersionNumbersFromReferencedAssembly(
            this GeneratorExecutionContext context,
            out IReadOnlyCollection<string> versionNumbers)
        {
            var appVersionNumber =
                context.Compilation.GetTypeByMetadataName($"{Constants.VersionsNamespace}.{Constants.VersionNumber}");
            if (appVersionNumber is null)
            {
                versionNumbers = null!;
                return false;
            }

            var appVersionValues = appVersionNumber.GetMembers()
                .Where(x => x.IsStatic && x.Kind is SymbolKind.Field)
                .Cast<IFieldSymbol>();

            versionNumbers = appVersionValues.Select(x => x.Name.Substring(Constants.Version.Length)).ToList();
            return true;
        }

        /// <summary>
        /// Gets applications versions.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="versionNumbers">Applications versions.</param>
        /// <returns>True if application versions are collected successfully. Otherwise, false.</returns>
        public static bool TryGetVersionNumbersFromContextAssembly(
            this GeneratorExecutionContext context,
            out IReadOnlyCollection<string> versionNumbers)
        {
            var appVersion = context.Compilation.GetTypeByMetadataName("RxBim.Nuke.Versions.AppVersion");
            if (appVersion is null || !context.IsSymbolInContext(appVersion))
            {
                versionNumbers = null!;
                return false;
            }

            versionNumbers = GetVersionNumbersFromSymbol(appVersion);
            return true;
        }

        private static IReadOnlyCollection<string> GetVersionNumbersFromSymbol(INamedTypeSymbol versionSymbol)
        {
            var appVersionValues = versionSymbol.GetMembers()
                .Where(x => x.IsStatic && x.Kind is SymbolKind.Field)
                .Cast<IFieldSymbol>();

            var numbers = new HashSet<string>();

            foreach (var appVersionValue in appVersionValues)
            {
                if (TryGetVersionNumberFromFieldValue(appVersionValue, out var number))
                    numbers.Add(number);
            }

            return numbers;
        }

        private static bool TryGetVersionNumberFromFieldValue(ISymbol fieldSymbol, out string verNumber)
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