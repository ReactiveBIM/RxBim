namespace RxBim.Application.Ribbon.Models
{
    using System;
    using Abstractions;
    using Di;

    /// <summary>
    /// Панель revit
    /// </summary>
    public abstract class PanelBase<TRibbon, TStackedItem, TButton> : RibbonBuilderBase<TRibbon>, IPanel
        where TRibbon : IRibbon
        where TStackedItem : StackedItemBase<TButton>
        where TButton : IButton
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">Лента</param>
        /// <param name="container"><see cref="IContainer"/></param>
        protected PanelBase(TRibbon ribbon, IContainer container)
            : base(ribbon, container)
        {
        }

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        public IPanel StackedItems(Action<IStackedItem> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var stackedItem = CreateStackedItem();
            action.Invoke(stackedItem);

            if (stackedItem.ItemsCount is < 2 or > 3)
            {
                throw new InvalidOperationException("You must create two or three items in the StackedItems");
            }

            AddStackedItemButtonsToRibbon(stackedItem);

            return this;
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public abstract IPanel Button(
            string name,
            string text,
            Type externalCommandType,
            Action<IButton> action = null);

        /// <summary>
        /// Create pull down button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public abstract IPanel PullDownButton(string name, string text, Action<IPulldownButton> action);

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        public abstract IPanel Separator();

        /// <inheritdoc />
        public abstract void AddAboutButton(
            string name,
            string text,
            string tabName,
            string panelName,
            IContainer container,
            Action<IAboutButton> action);

        /// <summary>
        /// Возвращает новую группу кнопок
        /// </summary>
        protected abstract TStackedItem CreateStackedItem();

        /// <summary>
        /// Добавляет кнопки на ленту CAD-приложения
        /// </summary>
        /// <param name="stackedItem">Сгруппированные кнопки</param>
        protected abstract void AddStackedItemButtonsToRibbon(TStackedItem stackedItem);
    }
}