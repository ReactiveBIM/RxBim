namespace PikTools.Application.Ui.Api.Builder
{
    /// <summary>
    /// Билдер панели Revit
    /// </summary>
    public abstract class RibbonBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        protected RibbonBuilder(Ribbon ribbon)
        {
            Ribbon = ribbon;
        }

        /// <summary>
        /// Панель
        /// </summary>
        protected Ribbon Ribbon { get; }

        /// <summary>
        /// Возвращает панель
        /// </summary>
        /// <returns></returns>
        public Ribbon And()
        {
            return Ribbon;
        }
    }
}