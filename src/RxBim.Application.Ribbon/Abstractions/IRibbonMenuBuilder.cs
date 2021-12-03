namespace RxBim.Application.Ribbon.Abstractions
{
    using Models.Configurations;
    using Shared.Abstractions;

    /// <summary>
    /// CAD platform-specific ribbon menu builder
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// Constructs CAD platform-specific ribbon
        /// </summary>
        /// <param name="ribbonConfig">Ribbon configuration</param>
        /// <param name="aboutShowService">Service for displaying the "About" window</param>
        void BuildRibbonMenu(Ribbon? ribbonConfig, IAboutShowService? aboutShowService);
    }
}