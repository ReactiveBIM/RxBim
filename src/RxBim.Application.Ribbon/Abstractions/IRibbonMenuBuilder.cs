namespace RxBim.Application.Ribbon.Abstractions
{
    using System;
    using Models.Configurations;

    /// <summary>
    /// CAD platform-specific ribbon menu builder
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// The menu has been created.
        /// </summary>
        event EventHandler MenuCreated;

        /// <summary>
        /// Constructs CAD platform-specific ribbon
        /// </summary>
        /// <param name="ribbonConfig">Ribbon configuration</param>
        void BuildRibbonMenu(Ribbon? ribbonConfig = null);
    }
}