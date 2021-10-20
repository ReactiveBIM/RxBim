namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Windows;
    using Models;
    using Ribbon.Abstractions;
    using Ribbon.Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Extensions for <see cref="TabBuilder"/>
    /// </summary>
    public static class TabExtensions
    {
        /// <summary>
        /// Returns <see cref="RibbonTab"/> for <see cref="ITabBuilder"/>
        /// </summary>
        /// <param name="tabBuilder"><see cref="ITabBuilder"/> object</param>
        /// <exception cref="InvalidOperationException">
        /// When <see cref="RibbonTab"/> for this <see cref="ITabBuilder"/> is not exists
        /// </exception>
        public static RibbonTab GetRibbonTab(this ITabBuilder tabBuilder)
        {
            if (tabBuilder is TabBuilder acTab)
                return ComponentManager.Ribbon.Tabs.Single(t => t.Id == acTab.Id);
            throw new InvalidOperationException("Can't get RibbonTab for this tab type!");
        }
    }
}