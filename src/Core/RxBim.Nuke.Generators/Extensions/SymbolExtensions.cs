namespace RxBim.Nuke.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Extensions for <see cref="ISymbol"/>.
    /// </summary>
    public static class SymbolExtensions
    {
        /// <summary>
        /// Returns attributes data for symbol.
        /// </summary>
        /// <param name="symbol">Symbol.</param>
        /// <param name="attTypeName">Name of attribute type.</param>
        public static IReadOnlyCollection<AttributeData> GetAttributes(this INamedTypeSymbol symbol, string attTypeName)
        {
            return symbol.GetAttributes().Where(x => x.AttributeClass?.Name == attTypeName).ToList();
        }

        /// <summary>
        /// Returns collection of properties names.
        /// </summary>
        /// <param name="symbol">Symbol for object with properties.</param>
        /// <param name="propertyTypeName">Name of properties type.</param>
        public static IEnumerable<string> GetPropertiesNames(this INamedTypeSymbol symbol, string propertyTypeName)
        {
            return symbol.GetMembers()
                .Where(x => x.Kind is SymbolKind.Property)
                .Cast<IPropertySymbol>()
                .Where(x => x.Type.Name == propertyTypeName)
                .Select(x => x.Name);
        }
    }
}