#pragma warning disable
namespace PikTools.Analyzers
{
    namespace PikTools.Analyzers
    {
        using System.Collections.Immutable;
        using System.Composition;
        using System.Threading.Tasks;
        using Fixes;
        using Microsoft.CodeAnalysis;
        using Microsoft.CodeAnalysis.CodeFixes;
        using SymbolActions;

        [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PikToolsAnalyzersCodeFixProvider)), Shared]
        public class PikToolsAnalyzersCodeFixProvider : CodeFixProvider
        {
            public sealed override ImmutableArray<string> FixableDiagnosticIds =>
                ImmutableArray.Create(CmdAction.DiagnosticId, CmdMethodAction.DiagnosticId, AppAction.StartDiagnosticId,
                    AppAction.ShutdownDiagnosticId, AppMethodAction.DiagnosticId);

            public sealed override FixAllProvider GetFixAllProvider()
            {
                // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
                return WellKnownFixAllProviders.BatchFixer;
            }

            public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
            {
                foreach (var diagnostic in context.Diagnostics)
                {
                    try
                    {
                        var diagnosticSpan = diagnostic.Location.SourceSpan;
                        var codeAction = await CodeActionFactory.GetCreator(diagnostic.Id)
                            .Create(context, diagnosticSpan);
                        context.RegisterCodeFix(codeAction, diagnostic);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
#pragma warning restore