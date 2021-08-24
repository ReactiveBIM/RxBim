namespace RxBim.Application.Ui.Api.Models
{
    using Abstractions;
    using Di;

    /// <summary>
    /// Строитель ленты
    /// </summary>
    public abstract class RibbonBuilderBase<T> : IRibbonBuilder
        where T : IRibbon
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        /// <param name="container">контейнер зависимостей</param>
        protected RibbonBuilderBase(T ribbon, IContainer container)
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