namespace PikTools.Analyzers.SymbolActions
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Инспекция проверяющая наличие методов Start и Shutdown приложения.
    /// </summary>
    public class AppAction
    {
        private const string StartDiagnosticId = Constants.DiagnosticId + "AppStart";

        private const string ShutdownDiagnosticId = Constants.DiagnosticId + "AppShutdown";

        private static readonly LocalizableString StartMethodTitle = "App type contains \"Start\" method.";

        private static readonly LocalizableString StartMethodMessageFormat =
            "App type '{0}' not contains \"Start\" method";

        private static readonly LocalizableString StartMethodDescription = "App type should contain \"Start\" method.";

        private static readonly LocalizableString ShutdownMethodTitle = "App type contains \"Shutdown\" method.";

        private static readonly LocalizableString ShutdownMethodMessageFormat =
            "App type '{0}' not contains \"Shutdown\" method";

        private static readonly LocalizableString ShutdownMethodDescription =
            "App type should contain \"Shutdown\" method.";

        /// <summary>
        /// Правило Start
        /// </summary>
        public DiagnosticDescriptor AppStartMethodRule { get; } = new DiagnosticDescriptor(
            StartDiagnosticId,
            StartMethodTitle,
            StartMethodMessageFormat,
            Constants.Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: StartMethodDescription);

        /// <summary>
        /// Правило Shutdown
        /// </summary>
        public DiagnosticDescriptor AppShutdownMethodRule { get; } = new DiagnosticDescriptor(
            ShutdownDiagnosticId,
            ShutdownMethodTitle,
            ShutdownMethodMessageFormat,
            Constants.Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: ShutdownMethodDescription);

        /// <summary>
        /// Проверка наличия метода Start
        /// </summary>
        /// <param name="context">контекст</param>
        public void AnalyzeApplicationStart(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name == "PikToolsApplication" &&
                namedTypeSymbol.MemberNames.All(x => x != "Start"))
            {
                var diagnostic =
                    Diagnostic.Create(AppStartMethodRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// Проверка наличия метода Shutdown
        /// </summary>
        /// <param name="context">контекст</param>
        public void AnalyzeApplicationShutDown(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name == "PikToolsApplication" &&
                namedTypeSymbol.MemberNames.All(x => x != "Shutdown"))
            {
                var diagnostic =
                    Diagnostic.Create(AppShutdownMethodRule,
                        namedTypeSymbol.Locations[0],
                        namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}