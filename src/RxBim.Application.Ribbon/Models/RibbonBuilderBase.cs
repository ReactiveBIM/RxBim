namespace RxBim.Application.Ribbon.Models
{
    using Abstractions;
    using Di;

    /// <summary>
    /// Строитель ленты
    /// </summary>
    public abstract class RibbonBuilderBase<T> : RibbonElement, IRibbonBuilder
        where T : IRibbon
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        /// <param name="id">Ribbon element identifier</param>
        /// <param name="container">контейнер зависимостей</param>
        protected RibbonBuilderBase(T ribbon, string id, IContainer container)
        : base(id)
        {
            Ribbon = ribbon;
            Container = container;
        }

        /// <summary>
        /// Панель
        /// </summary>
        protected T Ribbon { get; }

        /// <summary>
        /// Контейнер зависимостей
        /// </summary>
        protected IContainer Container { get; }

        /// <summary>
        /// Возвращает панель
        /// </summary>
        public IRibbon And()
        {
            return Ribbon;
        }
    }
}