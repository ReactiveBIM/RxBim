namespace PikTools.Analyzers.SymbolActions
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Инспекция возвращаемого значения комманды.
    /// </summary>
    public class CmdMethodAction
    {
        /// <summary>
        /// id
        /// </summary>
        public const string DiagnosticId = Constants.DiagnosticId + "CommandReturnType";

        private static readonly LocalizableString Title = "Method returns \"PluginResult\" method.";

        private static readonly LocalizableString MessageFormat =
            "Method '{0}' not returns \"PluginResult\" type";

        private static readonly LocalizableString Description =
            "\"ExecuteCommand\" method should return \"PluginResult\" type.";

        /// <summary>
        /// Правило
        /// </summary>
        public DiagnosticDescriptor Rule { get; } = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Constants.Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description);

        /// <summary>
        /// Проверка
        /// </summary>
        /// <param name="context">контекст</param>
        public void Analyze(SymbolAnalysisContext context)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.ContainingType.BaseType?.Name == "PikToolsCommand" &&
                method.Name == "ExecuteCommand" &&
                method.ReturnType.Name != Constants.PluginResult)
            {
                var diagnostic = Diagnostic.Create(Rule, method.Locations[0], method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}