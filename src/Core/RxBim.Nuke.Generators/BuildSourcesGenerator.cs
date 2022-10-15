﻿namespace RxBim.Nuke.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http.Headers;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Generates source for Build class.
    /// </summary>
    [Generator]
    public class BuildSourcesGenerator : ISourceGenerator
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
            var build = context.Compilation.GetTypeByMetadataName("Build");
            if (build is null || !context.CheckAssembly(build))
                return;

            var propsSource = GetBuildVerPropsSource();

            context.AddSource("Build.Versions.Properties.g.cs", propsSource);

            var gitHubActionsAttributes =
                build.GetAttributes().Where(x => x.AttributeClass is { Name: "GitHubActionsAttribute" }).ToList();

            if (!gitHubActionsAttributes.Any())
                return;

            if (!context.TryGetVersionNumbersFromExternalAssembly(out var versionNumbers))
                return;

            foreach (var actionsAttribute in gitHubActionsAttributes)
            {
                var source = GetBuildVerActionsSource(actionsAttribute, versionNumbers, out var actionName);
                if (!string.IsNullOrEmpty(actionName))
                    context.AddSource($"Build.Versions.Actions.{actionName}.g.cs", source);
            }
        }

        private static string GetBuildVerPropsSource()
        {
            return @"// <auto-generated>
using Nuke.Common;
using RxBim.Nuke.Versions;

/// <summary>
/// Build class.
/// </summary>
partial class Build : IVersionBuild
{
    /// <inheritdoc />
    [Parameter]
    public AppVersion AppVersion { get; set; }

    /// <inheritdoc />
    [Parameter]
    public AppVersionNumber AppVersionNumber { get; set; }
}";
        }

        private string GetBuildVerActionsSource(
            AttributeData actionsAttribute,
            IEnumerable<string> versionNumbers,
            out string actionName)
        {
            actionName = string.Empty;
            var syntaxReference = actionsAttribute.ApplicationSyntaxReference;
            if (syntaxReference is null)
                return string.Empty;

            var rootNode = syntaxReference.SyntaxTree.GetRoot();

            var usingLines = rootNode.GetUsingLines();

            var sourceSpan = syntaxReference.Span;
            var attNode = rootNode.FindNode(sourceSpan);
            var parsedCopy = attNode.GetNodeParsedCopy();

// #if DEBUG
//             Debugger.Launch();
// #endif
            if (!TryGetActionNameToken(parsedCopy, out var nameToken))
                return string.Empty;

            actionName = nameToken.ValueText;

            var attribs = new List<string>();

            foreach (var versionNumber in versionNumbers)
            {
                var text = $"\"{actionName}{versionNumber}\"";
                var valueText = $"{actionName}{versionNumber}";
                var newNameToken = SyntaxFactory.Token(nameToken.LeadingTrivia,
                    SyntaxKind.StringLiteralToken,
                    text,
                    valueText,
                    nameToken.TrailingTrivia);

                var newAttribNode = parsedCopy.ReplaceToken(nameToken, newNameToken);

                attribs.Add($"[{newAttribNode.GetText()}]");
            }

            return @$"{usingLines}
{string.Join(Environment.NewLine, attribs)}
partial class Build
{{
}}";
        }

        private bool TryGetActionNameToken(SyntaxNode parsedCopy, out SyntaxToken nameToken)
        {
            var nameArgumentSyntax = parsedCopy
                .GetChildNode<GlobalStatementSyntax>()
                ?.GetChildNode<ExpressionStatementSyntax>()
                ?.GetChildNode<InvocationExpressionSyntax>()
                ?.GetChildNode<ArgumentListSyntax>()
                ?.GetChildNode<ArgumentSyntax>(x => x.Expression is LiteralExpressionSyntax);

            if (nameArgumentSyntax is null)
            {
                nameToken = SyntaxFactory.MissingToken(SyntaxKind.StringLiteralToken);
                return false;
            }

            nameToken = ((LiteralExpressionSyntax)nameArgumentSyntax.Expression).Token;
            return true;
        }
    }
}