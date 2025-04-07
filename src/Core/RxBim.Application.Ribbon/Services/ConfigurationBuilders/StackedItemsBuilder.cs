namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Represents an items stack builder.
    /// </summary>
    public class StackedItemsBuilder : IStackedItemsBuilder
    {
        /// <summary>
        /// Max size of a stack.
        /// </summary>
        public const int MaxStackSize = 3;

        /// <summary>
        /// Builds StackedItems.
        /// </summary>
        public StackedItems StackedItems { get; } = new();

        /// <inheritdoc />
        public IStackedItemsBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommandButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);

            return AddItem(buttonBuilder.Build());
        }

        /// <inheritdoc />
        public IStackedItemsBuilder PullDownButton(string name, Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            return AddItem(pulldownButton.Build());
        }

        /// <inheritdoc />
        public IStackedItemsBuilder ComboBox(string name, Action<IComboBoxBuilder> builder)
        {
            var combobox = new ComboBoxBuilder(name);
            builder.Invoke(combobox);
            return AddItem(combobox.Build());
        }

        /// <summary>
        /// Adds a item in the stack.
        /// </summary>
        /// <param name="item">Ribbon item.</param>
        internal IStackedItemsBuilder AddItem(IRibbonPanelItem item)
        {
            if (StackedItems.Items.Count == MaxStackSize)
                throw new InvalidOperationException($"Can't create more than {MaxStackSize} items in the StackedItem!");
            StackedItems.Items.Add(item);
            return this;
        }
    }
}