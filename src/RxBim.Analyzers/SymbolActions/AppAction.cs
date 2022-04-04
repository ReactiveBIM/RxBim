namespace RxBim.Analyzers.SymbolActions
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using static Constants;

    /// <summary>
    /// Inspection checks methods <see cref="Start"/> и <see cref="Shutdown"/> in RxBimApplication
    /// </summary>
    public class AppAction
    {
        /// <summary>
        /// id
        /// </summary>
        public const string StartDiagnosticId = DiagnosticId + "AppStart";

        /// <summary>
        /// id
        /// </summary>
        public const string ShutdownDiagnosticId = DiagnosticId + "AppShutdown";

        private static readonly LocalizableString StartMethodTitle = $"App type contains \"{Start}\" method.";

        private static readonly LocalizableString StartMethodMessageFormat =
            $"App type '{{0}}' not contains \"{Start}\" method";

        private static readonly LocalizableString StartMethodDescription = $"App type should contain \"{Start}\" method.";

        private static readonly LocalizableString ShutdownMethodTitle = $"App type contains \"{Shutdown}\" method.";

        private static readonly LocalizableString ShutdownMethodMessageFormat =
            $"App type '{{0}}' not contains \"{Shutdown}\" method";

        private static readonly LocalizableString ShutdownMethodDescription =
            $"App type should contain \"{Shutdown}\" method.";

        /// <summary>
        /// Rule for checking Start method
        /// </summary>
        public DiagnosticDescriptor AppStartMethodRule { get; } = new DiagnosticDescriptor(
            StartDiagnosticId,
            StartMethodTitle,
            StartMethodMessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: StartMethodDescription);

        /// <summary>
        /// Rule for checking Shutdown method
        /// </summary>
        public DiagnosticDescriptor AppShutdownMethodRule { get; } = new DiagnosticDescriptor(
            ShutdownDiagnosticId,
            ShutdownMethodTitle,
            ShutdownMethodMessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: ShutdownMethodDescription);

        /// <summary>
        /// Checks for availability of Start method
        /// </summary>
        /// <param name="context">context</param>
        public void AnalyzeApplicationStart(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType?.Name == RxBimApplication &&
                namedTypeSymbol.MemberNames.All(x => x != Start))
            {
                var diagnostic =
                    Diagnostic.Create(AppStartMethodRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// Checks for availability Shutdown method
        /// </summary>
        /// <param name="context">context</param>
        public void AnalyzeApplicationShutDown(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType?.Name == RxBimApplication &&
                namedTypeSymbol.MemberNames.All(x => x != Shutdown))
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