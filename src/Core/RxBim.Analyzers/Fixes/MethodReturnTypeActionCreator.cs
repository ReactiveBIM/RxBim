namespace RxBim.Analyzers.Fixes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using Microsoft.CodeAnalysis.Text;

    /// <inheritdoc />
    public abstract class MethodReturnTypeActionCreator : IActionCreator
    {
        /// <inheritdoc />
        public abstract string DiagnosticId { get; }

        /// <inheritdoc />
        public async Task<CodeAction?> Create(CodeFixContext context, TextSpan diagnosticSpan)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var parent = root?.FindToken(diagnosticSpan.Start).Parent;
            if (parent is null)
                return null;

            const string title = "Fix method return type";
            return CodeAction.Create(
                title: title,
                createChangedSolution: _ => GetSolutionAsync(
                    root!,
                    context.Document,
                    parent.AncestorsAndSelf()
                        .OfType<MethodDeclarationSyntax>()
                        .First()),
                equivalenceKey: title);
        }

        private Task<Solution> GetSolutionAsync(SyntaxNode root, Document document, MethodDeclarationSyntax methodDecl)
        {
            var newClassDecl = methodDecl.WithReturnType(SyntaxFactory.ParseTypeName(Constants.PluginResult))
                .WithAdditionalAnnotations(Formatter.Annotation);
            return Task.FromResult(document.WithSyntaxRoot(root.ReplaceNode(methodDecl, newClassDecl))
                .Project.Solution);
        }
    }
}