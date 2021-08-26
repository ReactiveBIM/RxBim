namespace RxBim.Application.Ribbon.Models
{
    using Abstractions;
    using Di;

    /// <inheritdoc />
    public abstract class RibbonBase : IRibbon
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonBase"/> class.
        /// </summary>
        /// <param name="container">Контейнер</param>
        protected RibbonBase(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc />
        public ITab Tab(string tabTitle = null)
        {
            if (!string.IsNullOrEmpty(tabTitle))
            {
                if (!TabIsExists(tabTitle))
                {
                    CreateTabAndAddToRibbon(tabTitle);
                }
            }

            return GetTab(tabTitle, _container);
        }

        /// <summary>
        /// Возвращает истину, если вкладка уже есть на ленте
        /// </summary>
        /// <param name="tabTitle">Название вкладки</param>
        protected abstract bool TabIsExists(string tabTitle);

        /// <summary>
        /// Создаёт вкладку и добавляет её на ленту
        /// </summary>
        /// <param name="tabTitle">Название вкладки</param>
        protected abstract void CreateTabAndAddToRibbon(string tabTitle);

        /// <summary>
        /// Возвращает вкладку
        /// </summary>
        /// <param name="title">Название вкладки</param>
        /// <param name="container">Контейнер</param>
        protected abstract ITab GetTab(string title, IContainer container);
    }
}