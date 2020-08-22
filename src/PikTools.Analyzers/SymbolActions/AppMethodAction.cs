namespace PikTools.Analyzers.SymbolActions
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    public class AppMethodAction
    {
        private static readonly LocalizableString Title = "Method returns \"PluginResult\" method.";

        private static readonly LocalizableString MessageFormat =
            "Method '{0}' not returns \"PluginResult\" type";

        private static readonly LocalizableString Description =
            "\"ExecuteCommand\" method should return \"PluginResult\" type.";

        private const string DiagnosticId = Constants.DiagnosticId + "AppMethodReturType";

        public readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Constants.Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description);

        public void AnalyzeAppStartMethods(SymbolAnalysisContext context)
        {
            AnalyzeMethod(context, "Start");
        }

        public void AnalyzeAppShutdownMethods(SymbolAnalysisContext context)
        {
            AnalyzeMethod(context, "Shutdown");
        }

        private void AnalyzeMethod(SymbolAnalysisContext context, string methodName)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.ContainingType.BaseType?.Name == "PikToolsApplication" &&
                method.Name == methodName &&
                method.ReturnType.Name != "PluginResult")
            {
                var diagnostic = Diagnostic.Create(Rule, method.Locations[0], method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}