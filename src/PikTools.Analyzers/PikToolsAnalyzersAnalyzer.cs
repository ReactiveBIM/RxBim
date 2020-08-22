namespace PikTools.Analyzers
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using SymbolActions;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PikToolsAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        private readonly CmdAction _cmdAction;
        private readonly AppMethodAction _appMethodAction;
        private readonly CmdMethodAction _cmdMethodAction;
        private readonly AppAction _appAction;

        public PikToolsAnalyzersAnalyzer()
        {
            _cmdAction = new CmdAction();
            _appMethodAction = new AppMethodAction();
            _cmdMethodAction = new CmdMethodAction();
            _appAction = new AppAction();
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(_cmdAction.Rule, _appMethodAction.Rule, _cmdMethodAction.Rule,
                _appAction.AppStartMethodRule, _appAction.AppShutdownMethodRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(_cmdAction.Analyze, SymbolKind.NamedType);
            context.RegisterSymbolAction(_appAction.AnalyzeApplicationStart, SymbolKind.NamedType);
            context.RegisterSymbolAction(_appAction.AnalyzeApplicationShutDown, SymbolKind.NamedType);
            context.RegisterSymbolAction(_cmdMethodAction.Analyze, SymbolKind.Method);
            context.RegisterSymbolAction(_appMethodAction.AnalyzeAppStartMethods, SymbolKind.Method);
            context.RegisterSymbolAction(_appMethodAction.AnalyzeAppShutdownMethods, SymbolKind.Method);
        }
    }
}