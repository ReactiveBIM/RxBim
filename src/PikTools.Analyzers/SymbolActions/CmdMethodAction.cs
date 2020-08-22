namespace PikTools.Analyzers.SymbolActions
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    public class CmdMethodAction
    {
        private static readonly LocalizableString Title = "Method returns \"PluginResult\" method.";

        private static readonly LocalizableString MessageFormat =
            "Method '{0}' not returns \"PluginResult\" type";

        private static readonly LocalizableString Description =
            "\"ExecuteCommand\" method should return \"PluginResult\" type.";

        private const string DiagnosticId = Constants.DiagnosticId + "CommandReturnType";

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Constants.Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public void Analyze(SymbolAnalysisContext context)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.ContainingType.BaseType?.Name == "PikToolsCommand" &&
                method.Name == "ExecuteCommand" &&
                method.ReturnType.Name != "PluginResult")
            {
                var diagnostic = Diagnostic.Create(Rule, method.Locations[0], method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}