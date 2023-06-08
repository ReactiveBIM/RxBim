namespace RxBim.Nuke.Generators
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Extensions for <see cref="SyntaxNode"/>.
    /// </summary>
    public static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Returns child node.
        /// </summary>
        /// <param name="node"><see cref="SyntaxNode"/> object.</param>
        /// <param name="predicate">Child node check predicate.</param>
        /// <typeparam name="T">Child node type.</typeparam>
        public static T? GetChildNode<T>(this SyntaxNode node, Predicate<T>? predicate = null)
            where T : SyntaxNode
        {
            var syntaxNodes = node
                .ChildNodes()
                .OfType<T>();

            return predicate is null
                ? syntaxNodes.FirstOrDefault()
                : syntaxNodes.FirstOrDefault(x => predicate(x));
        }

        /// <summary>
        /// Returns using directive lines from <see cref="SyntaxNode"/> object.
        /// </summary>
        /// <param name="rootNode"><see cref="SyntaxNode"/> object.</param>
        public static string GetUsingDirectives(this SyntaxNode rootNode)
        {
            return string.Join(string.Empty,
                rootNode.ChildNodes().OfType<UsingDirectiveSyntax>().Select(x => x.GetText().ToString()));
        }
    }
}