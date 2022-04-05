namespace RxBim.Application.Ribbon.Abstractions
{
    using System.Reflection;

    /// <summary>
    /// Factory for <see cref="IRibbonMenuBuilder"/>.
    /// </summary>
    public interface IRibbonMenuBuilderFactory
    {
        /// <summary>
        /// Creates ribbon menu builder.
        /// </summary>
        /// <param name="menuAssembly">Menu defining assembly.</param>
        IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly);
    }
}