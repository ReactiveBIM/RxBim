namespace RxBim.Application.Ribbon.Autocad.Extensions
{
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
    }
}