namespace PikTools.Analyzers
{
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class PikToolsAnalyzersAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(CmdRules.CommandRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeCommand, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeApplicationStart, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeApplicationShutDown, SymbolKind.NamedType);
            context.RegisterSymbolAction(AnalyzeCmdMethod, SymbolKind.Method);
            context.RegisterSymbolAction(AnalyzeAppStartMethods, SymbolKind.Method);
            context.RegisterSymbolAction(AnalyzeAppShutdownMethods, SymbolKind.Method);
        }

        private void AnalyzeAppStartMethods(SymbolAnalysisContext context)
        {
            AnalyzeMethod(context, "Start");
        }

        private void AnalyzeAppShutdownMethods(SymbolAnalysisContext context)
        {
            AnalyzeMethod(context, "Shutdown");
        }

        private void AnalyzeMethod(SymbolAnalysisContext context, string methodName)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.ContainingType.BaseType?.Name == "PikToolsApplication" &&
                method.Name == methodName &&
                method.ReturnType.Name != "PluginResult")
            {
                var diagnostic = Diagnostic.Create(MethodRules.MethodRule, method.Locations[0], method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeCmdMethod(SymbolAnalysisContext context)
        {
            var method = (IMethodSymbol)context.Symbol;

            if (method.ContainingType.BaseType?.Name == "PikToolsCommand" &&
                method.Name == "ExecuteCommand" &&
                method.ReturnType.Name != "PluginResult")
            {
                var diagnostic = Diagnostic.Create(MethodRules.MethodRule, method.Locations[0], method.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeApplicationStart(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name == "PikToolsApplication" &&
                namedTypeSymbol.MemberNames.All(x => x != "Start"))
            {
                var diagnostic =
                    Diagnostic.Create(AppRules.AppStartMethodRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        private void AnalyzeApplicationShutDown(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name == "PikToolsApplication" &&
                namedTypeSymbol.MemberNames.All(x => x != "Shutdown"))
            {
                var diagnostic =
                    Diagnostic.Create(AppRules.AppShutdownMethodRule, namedTypeSymbol.Locations[0],
                        namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }

        private static void AnalyzeCommand(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.BaseType.Name == "PikToolsCommand" &&
                namedTypeSymbol.MemberNames.All(x => x != "ExecuteCommand"))
            {
                var diagnostic =
                    Diagnostic.Create(CmdRules.CommandRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}