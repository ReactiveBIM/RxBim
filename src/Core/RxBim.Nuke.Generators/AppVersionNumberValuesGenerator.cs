﻿namespace RxBim.Nuke.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates AppVersionNumber values.
    /// </summary>
    [Generator]
    public class AppVersionNumberValuesGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
// #if DEBUG
//             Debugger.Launch();
// #endif
        }

        /// <inheritdoc />
        public void Execute(GeneratorExecutionContext context)
        {
            var appVersion = context.Compilation.GetTypeByMetadataName("RxBim.Nuke.Versions.AppVersion");
            if (appVersion is null || !context.CheckAssembly(appVersion))
                return;

            var versionNumbers = GetVersionNumbers(appVersion);

            var verNumSource = GetSource(versionNumbers);
            context.AddSource("AppVersionNumber.g.cs", verNumSource);
        }

        private static string GetSource(IEnumerable<string> versionNumbers)
        {
            var values = string.Join(Environment.NewLine,
                versionNumbers.Select(x =>
                    $@"        public static AppVersionNumber Version{x} = new() {{ Value = ""{x}"" }};"));

            return $@"// <auto-generated/>
#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211

namespace RxBim.Nuke.Versions

{{
    public partial class AppVersionNumber
    {{
{values}
    }}
}}";
        }

        private static IEnumerable<string> GetVersionNumbers(INamespaceOrTypeSymbol appVersion)
        {
            var appVersionValues = appVersion.GetMembers()
                .Where(x => x.IsStatic && x.Kind is SymbolKind.Field)
                .Cast<IFieldSymbol>();

            var numbers = new HashSet<string>();

            foreach (var appVersionValue in appVersionValues)
            {
                if (TryGetVersionNumber(appVersionValue, out var number))
                    numbers.Add(number);
            }

            return numbers;
        }

        private static bool TryGetVersionNumber(ISymbol fieldSymbol, out string verNumber)
        {
            verNumber = string.Empty;

            var sourceSpan = fieldSymbol.Locations.First().SourceSpan;
            var syntaxReference = fieldSymbol.DeclaringSyntaxReferences.FirstOrDefault(x =>
                x.Span.Start <= sourceSpan.Start && x.Span.End >= sourceSpan.End);

            if (syntaxReference?.GetSyntax().FindNode(sourceSpan) is not VariableDeclaratorSyntax
                fieldDeclarationSyntax)
                return false;

            var appVerArgCreation = fieldDeclarationSyntax.DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .FirstOrDefault(x => x.Type is IdentifierNameSyntax { Identifier: { Text: "ApplicationVersion" } });

            var arg = appVerArgCreation?.ArgumentList?.Arguments.FirstOrDefault();
            if (arg?.Expression is not LiteralExpressionSyntax expressionSyntax)
                return false;

            verNumber = expressionSyntax.Token.ValueText;

            return !string.IsNullOrEmpty(verNumber);
        }
    }
}