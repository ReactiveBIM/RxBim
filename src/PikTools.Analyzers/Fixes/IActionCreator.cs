namespace PikTools.Analyzers.Fixes
{
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// Создает действия для исправления
    /// </summary>
    public interface IActionCreator
    {
        /// <summary>
        /// идентификатор диагностики
        /// </summary>
        public string DiagnosticId { get; }

        /// <summary>
        /// Создает действие
        /// </summary>
        /// <param name="context">контекст</param>
        /// <param name="diagnosticSpan">span</param>
        /// <returns></returns>
        public Task<CodeAction> Create(CodeFixContext context, TextSpan diagnosticSpan);
    }
}