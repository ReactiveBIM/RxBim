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
        private IRibbon _ribbon;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBuildServiceBase"/> class.
        /// </summary>
        /// <param name="ribbonFactory">Ribbon Factory</param>
        /// <param name="action">Action to build a ribbon</param>
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
        /// Internal menu building method
        /// </summary>
        protected void BuildMenuInternal()
        {
            _ribbon ??= _ribbonFactory.Create(_container, _action);
        }
    }
}