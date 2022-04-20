namespace RxBim.Analyzers.Fixes
{
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Action for code fix.
    /// </summary>
    public interface IActionCreator
    {
        /// <summary>
        /// Diagnostic identifier.
        /// </summary>
        public string DiagnosticId { get; }

        /// <summary>
        /// Creates action.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="diagnosticSpan">A span.</param>
        public Task<CodeAction> Create(CodeFixContext context, TextSpan diagnosticSpan);
    }
}