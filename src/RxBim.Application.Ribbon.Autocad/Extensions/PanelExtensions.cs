namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Windows;
    using Models;
    using Ribbon.Abstractions;

    /// <summary>
    /// Extensions for <see cref="IPanelBuilder"/>
    /// </summary>
    public static class PanelExtensions
    {
        /// <summary>
        /// Switches the panel to SlideOut fill mode
        /// </summary>
        /// <param name="panelBuilder"><see cref="IPanelBuilder"/> object</param>
        public static IPanelBuilder SlideOut(this IPanelBuilder panelBuilder)
        {
            if (panelBuilder is PanelBuilder cadPanel)
            {
                cadPanel.SwitchToSlideOut();
            }

            return panelBuilder;
        }

        /// <summary>
        /// Returns <see cref="RibbonPanel"/> for <see cref="IPanelBuilder"/>
        /// </summary>
        /// <param name="panelBuilder"><see cref="IPanelBuilder"/> object</param>
        internal static RibbonPanel GetRibbonPanel(this IPanelBuilder panelBuilder)
        {
            if (panelBuilder is PanelBuilder acPanel)
                return panelBuilder.TabBuilder.GetRibbonTab().Panels.Single(p => p.Id == acPanel.Id);
            throw new InvalidOperationException("Can't get RibbonPanel for this panel type!");
        }
    }
}