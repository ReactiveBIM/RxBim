namespace RxBim.Application.Ribbon.Models
{
    using System;
    using Abstractions;
    using Di;

    /// <summary>
    /// Ribbon panel base implementation
    /// </summary>
    /// <typeparam name="TRibbon">Ribbon implementation type for a specific type of CAD platform</typeparam>
    /// <typeparam name="TStackedItems">StackedItems implementation type for a specific type of CAD platform</typeparam>
    /// <typeparam name="TButton">Button implementation type for a specific type of CAD platform</typeparam>
    public abstract class PanelBase<TRibbon, TStackedItems, TButton> : RibbonBuilderBase<TRibbon>, IPanel
        where TRibbon : IRibbon
        where TStackedItems : StackedItemsBase<TButton>
        where TButton : IButton
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">Ribbon object</param>
        /// <param name="container"><see cref="IContainer"/></param>
        /// <param name="tab"><see cref="Tab"/></param>
        protected PanelBase(TRibbon ribbon, IContainer container, ITab tab)
            : base(ribbon, container)
        {
            Tab = tab;
        }

        /// <inheritdoc />
        public ITab Tab { get; }

        /// <summary>
        /// Create new stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        public IPanel StackedItems(Action<IStackedItems> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var stackedItems = CreateStackedItems();
            action.Invoke(stackedItems);

            if (stackedItems.ItemsCount is < 2 or > 3)
            {
                throw new InvalidOperationException("You must create two or three items in the StackedItems");
            }

            AddStackedButtonsToRibbon(stackedItems);

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
        /// Returns a new stack of ribbon items
        /// </summary>
        protected abstract TStackedItems CreateStackedItems();

        /// <summary>
        /// Add stacked buttons to ribbon in specific CAD platform
        /// </summary>
        /// <param name="stackedItems">Stack of ribbon items</param>
        protected abstract void AddStackedButtonsToRibbon(TStackedItems stackedItems);
    }
}