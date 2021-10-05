namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Di;

    /// <inheritdoc />
    public abstract class MenuBuildServiceBase : IMenuBuildService
    {
        private readonly IRibbonFactory _ribbonFactory;
        private readonly Action<IRibbon> _action;
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBuildServiceBase"/> class.
        /// </summary>
        /// <param name="ribbonFactory">Фабрика ленты</param>
        /// <param name="action">Действие по построению ленты</param>
        protected MenuBuildServiceBase(
            IRibbonFactory ribbonFactory,
            Action<IRibbon> action)
        {
            _ribbonFactory = ribbonFactory;
            _action = action;
        }

        /// <inheritdoc />
        public virtual void BuildMenu(IContainer container)
        {
            _container = container;
            BuildMenuInternal();
        }

        /// <summary>
        /// Внутренний метод построения меню
        /// </summary>
        protected void BuildMenuInternal()
        {
            var ribbon = _ribbonFactory.Create(_container);
            if (ribbon.IsValid)
            {
                _action(ribbon);
            }
        }
    }
}