namespace RxBim.Application.Ribbon.Abstractions
{
    using System.Reflection;

    /// <summary>
    /// Defines a factory for <see cref="IRibbonMenuBuilder"/>.
    /// </summary>
    public interface IRibbonMenuBuilderFactory
    {
        /// <summary>
        /// Creates a ribbon menu builder.
        /// </summary>
        /// <param name="menuAssembly">An assembly containing menu definition.</param>
        IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly);
    }
}