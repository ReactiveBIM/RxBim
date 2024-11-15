namespace RxBim.Application.Ribbon
{
    using System;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    /// <summary>
    /// Utility class for creating the plugin ribbon.
    /// </summary>
    public class MenuBuilderUtility
    {
        /// <summary>
        /// Builds the plugin menu.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        public static void BuildMenu(IServiceProvider serviceProvider)
        {
            try
            {
                var builder = serviceProvider.GetService<IRibbonMenuBuilder>();
                if (builder != null)
                {
                    var ribbonConfiguration = serviceProvider.GetService<Ribbon>();
                    builder.BuildRibbonMenu(ribbonConfiguration);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Failed to build ribbon", e);
            }
        }
    }
}