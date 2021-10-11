namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Windows;
    using Ribbon.Abstractions;

    /// <summary>
    /// Extensions for <see cref="Models.Tab"/>
    /// </summary>
    public static class TabExtensions
    {
        /// <summary>
        /// Returns <see cref="RibbonTab"/> for <see cref="ITab"/>
        /// </summary>
        /// <param name="tab"><see cref="ITab"/> object</param>
        /// <exception cref="InvalidOperationException">When <see cref="RibbonTab"/> for this <see cref="ITab"/> is not exists</exception>
        public static RibbonTab GetRibbonTab(this ITab tab)
        {
            return ComponentManager.Ribbon.Tabs.Single(t => t.Id == tab.Id);
        }
    }
}