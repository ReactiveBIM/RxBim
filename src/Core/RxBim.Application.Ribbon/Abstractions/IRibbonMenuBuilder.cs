namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;

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

        /// <summary>
        /// Initializes the service.
        /// </summary>
        /// <param name="menuAssembly">Menu defining assembly.</param>
        void Initialize(Assembly menuAssembly);
    }
}