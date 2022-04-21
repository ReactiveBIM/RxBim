namespace RxBim.Application.Ribbon
{
    using RxBim.Shared.Abstractions;

    /// <summary>
    /// Defines a CAD platform-specific ribbon menu builder.
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// Constructs a CAD platform-specific ribbon.
        /// </summary>
        /// <param name="ribbonConfig">The ribbon configuration.</param>
        /// <param name="aboutShowService">The Service displaying an "About" window.</param>
        void BuildRibbonMenu(Ribbon? ribbonConfig, IAboutShowService? aboutShowService);
    }
}