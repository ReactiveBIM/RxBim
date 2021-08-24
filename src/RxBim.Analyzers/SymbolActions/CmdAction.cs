namespace RxBim.Analyzers.SymbolActions
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using static Constants;

    /// <summary>
    /// Инспекция проверяющая наличие метода <see cref="ExecuteCommand"/>
    /// </summary>
    public class CmdAction
    {
        /// <summary>
        /// шв
        /// </summary>
        public const string DiagnosticId = Constants.DiagnosticId + "Command";

        private static readonly LocalizableString Title = $"Command type contains \"{ExecuteCommand}\" method.";

        private static readonly LocalizableString MessageFormat =
            $"Command type '{{0}}' not contains \"{ExecuteCommand}\" method";

        private static readonly LocalizableString
            Description = $"Command type should contain \"{ExecuteCommand}\" method.";

        /// <summary>
        /// Правило
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
        /// Проверка
        /// </summary>
        /// <param name="context">контекст</param>
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