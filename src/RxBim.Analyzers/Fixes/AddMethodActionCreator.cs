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
    public abstract class AddMethodActionCreator : IActionCreator
    {
        /// <inheritdoc />
        public abstract string DiagnosticId { get; }

        /// <summary>
        /// Title
        /// </summary>
        protected abstract string Title { get; }

        /// <summary>
        /// Имя метода для генерации
        /// </summary>
        protected abstract string MethodName { get; }

        /// <inheritdoc />
        public async Task<CodeAction> Create(CodeFixContext context, TextSpan diagnosticSpan)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            return CodeAction.Create(
                title: Title,
                createChangedSolution: c =>
                    GetSolutionAsync(root,
                        context.Document,
                        root.FindToken(diagnosticSpan.Start)
                            .Parent.AncestorsAndSelf()
                            .OfType<TypeDeclarationSyntax>()
                            .First()),
                equivalenceKey: Title);
        }

        private Task<Solution> GetSolutionAsync(
            SyntaxNode root,
            Document document,
            TypeDeclarationSyntax typeDecl)
        {
            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                    SyntaxFactory.TokenList(new SyntaxToken[] { SyntaxFactory.Token(SyntaxKind.PublicKeyword) }),
                    SyntaxFactory.ParseTypeName(Constants.PluginResult),
                    null,
                    SyntaxFactory.Identifier(MethodName),
                    null,
                    SyntaxFactory.ParameterList(),
                    SyntaxFactory.List<TypeParameterConstraintClauseSyntax>(),
                    SyntaxFactory.Block(SyntaxFactory.ParseStatement("return PluginResult.Succeeded;")),
                    SyntaxFactory.Token(SyntaxKind.None))
                .WithAdditionalAnnotations(Formatter.Annotation);

            var newClassDecl = typeDecl.AddMembers(method);
            return Task.FromResult(document.WithSyntaxRoot(root.ReplaceNode(typeDecl, newClassDecl)).Project.Solution);
        }
    }
}