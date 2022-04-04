namespace RxBim.Analyzers.SymbolActions
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using static Constants;

    /// <summary>
    /// Inspectioin checks <see cref="ExecuteCommand"/> method of RxBimCommand
    /// </summary>
    public class CmdAction
    {
        /// <summary>
        /// Id
        /// </summary>
        public const string DiagnosticId = Constants.DiagnosticId + "Command";

        private static readonly LocalizableString Title = $"Command type contains \"{ExecuteCommand}\" method.";

        private static readonly LocalizableString MessageFormat =
            $"Command type '{{0}}' not contains \"{ExecuteCommand}\" method";

        private static readonly LocalizableString
            Description = $"Command type should contain \"{ExecuteCommand}\" method.";

        /// <summary>
        /// Rule
        /// </summary>
        public DiagnosticDescriptor Rule { get; } = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: Description);

        /// <summary>
        /// Checks for availability of ExecuteCommand method
        /// </summary>
        /// <param name="context">context</param>
        public void Analyze(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType?.Name == RxBimCommand &&
                namedTypeSymbol.MemberNames.All(x => x != ExecuteCommand))
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}