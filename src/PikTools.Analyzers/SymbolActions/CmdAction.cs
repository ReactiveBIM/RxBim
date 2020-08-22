namespace PikTools.Analyzers.SymbolActions
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    public class CmdAction
    {
        private static readonly LocalizableString Title = "Command type contains \"ExecuteCommand\" method.";

        private static readonly LocalizableString MessageFormat =
            "Command type '{0}' not contains \"ExecuteCommand\" method";

        private static readonly LocalizableString
            Description = "Command type should contain \"ExecuteCommand\" method.";

        private const string DiagnosticId = Constants.DiagnosticId + "Command";

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Constants.Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public void Analyze(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name == "PikToolsCommand" &&
                namedTypeSymbol.MemberNames.All(x => x != "ExecuteCommand"))
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}