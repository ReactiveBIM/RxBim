namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Windows;
    using Models;
    using Ribbon.Abstractions;

    /// <summary>
    /// Extensions for <see cref="IPanel"/>
    /// </summary>
    public static class PanelExtensions
    {
        /// <summary>
        /// Switches the panel to SlideOut fill mode
        /// </summary>
        /// <param name="panel"><see cref="IPanel"/> object</param>
        public static IPanel SlideOut(this IPanel panel)
        {
            if (panel is Panel cadPanel)
            {
                cadPanel.SwitchToSlideOut();
            }

            return panel;
        }

        /// <summary>
        /// Returns <see cref="RibbonPanel"/> for <see cref="IPanel"/>
        /// </summary>
        /// <param name="panel"><see cref="IPanel"/> object</param>
        internal static RibbonPanel GetRibbonPanel(this IPanel panel)
        {
            if (panel is Panel acPanel)
                return panel.Tab.GetRibbonTab().Panels.Single(p => p.Id == acPanel.Id);
            throw new InvalidOperationException("Can't get RibbonPanel for this panel type!");
        }
    }
}