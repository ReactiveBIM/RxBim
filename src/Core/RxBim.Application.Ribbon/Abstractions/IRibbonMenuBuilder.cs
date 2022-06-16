namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Defines a CAD platform-specific ribbon menu builder.
    /// </summary>
    public interface IRibbonMenuBuilder
    {
        /// <summary>
        /// The menu has been created.
        /// </summary>
        event EventHandler MenuCreated;

        /// <summary>
        /// Constructs a CAD platform-specific ribbon.
        /// </summary>
        /// <param name="ribbonConfig">The ribbon configuration.</param>
        void BuildRibbonMenu(Ribbon? ribbonConfig = null);
    }
}