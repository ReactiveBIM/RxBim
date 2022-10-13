namespace RxBim.Nuke.Generators
{
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Extensions for checks.
    /// </summary>
    public static class CheckExtensions
    {
        /// <summary>
        /// Returns true if the symbol is in context assembly. Otherwise, returns false.
        /// </summary>
        /// <param name="context">Generator context.</param>
        /// <param name="symbol">The symbol to check.</param>
        public static bool CheckAssembly(this GeneratorExecutionContext context, ISymbol symbol)
        {
            return context.Compilation.Assembly.Equals(symbol.ContainingAssembly, SymbolEqualityComparer.Default);
        }
    }
}