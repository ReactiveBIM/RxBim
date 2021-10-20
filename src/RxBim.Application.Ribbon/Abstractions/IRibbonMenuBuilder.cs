namespace RxBim.Application.Ribbon.Abstractions
{
    using Models.Configurations;

    /// <summary>
    /// CAD platform-specific ribbon menu builder
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// Constructs CAD platform-specific ribbon
        /// </summary>
        /// <param name="ribbonConfig">Ribbon configuration</param>
        void BuildRibbonMenu(Ribbon ribbonConfig);
    }
}