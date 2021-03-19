namespace PikTools.Application.Ui.Api.Builder
{
    using PikTools.Di;

    /// <summary>
    /// Билдер панели Revit
    /// </summary>
    public abstract class RibbonBuilder
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        /// <param name="container">контейнер зависимостей</param>
        protected RibbonBuilder(Ribbon ribbon, IContainer container)
        {
            Ribbon = ribbon;
            Container = container;
        }

        /// <summary>
        /// Панель
        /// </summary>
        protected Ribbon Ribbon { get; }

        /// <summary>
        /// Контейнер зависимостей
        /// </summary>
        protected IContainer Container { get; }

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