namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System.Linq;
    using Autodesk.Windows;
    using Models;
    using Ribbon.Abstractions;

    /// <summary>
    /// Расширения для панели
    /// </summary>
    public static class PanelExtensions
    {
        /// <summary>
        /// Переключает панель в режим заполнения выдвигающейся части
        /// </summary>
        /// <param name="panel">Панель</param>
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
        public static RibbonPanel GetRibbonPanel(this IPanel panel)
        {
            return panel.Tab.GetRibbonTab().Panels.Single(p => p.Id == panel.Id);
        }
    }
}