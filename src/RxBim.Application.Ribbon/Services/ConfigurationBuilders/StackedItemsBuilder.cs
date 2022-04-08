namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

    /// <inheritdoc />
    public class StackedItemsBuilder : IStackedItemsBuilder
    {
        /// <summary>
        /// Max size of a stack
        /// </summary>
        public const int MaxStackSize = 3;

        /// <summary>
        /// Builds StackedItems
        /// </summary>
        public StackedItems StackedItems { get; } = new();

        /// <inheritdoc />
        public IStackedItemsBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            action?.Invoke(buttonBuilder);

            return AddElement(buttonBuilder.BuildingButton);
        }

        /// <inheritdoc />
        public IStackedItemsBuilder AddCommandButton<T>(string name, Action<IButtonBuilder>? action = null)
        {
            return AddCommandButton(name, typeof(T), action);
        }

        /// <inheritdoc />
        public IStackedItemsBuilder AddPullDownButton(string name, Action<IPulldownButtonBuilder> action)
        {
            var builder = new PulldownButtonBuilder(name);
            action.Invoke(builder);
            return AddElement(builder.BuildingButton);
        }

        /// <inheritdoc />
        public IStackedItemsBuilder AddElement(IRibbonPanelElement element)
        {
            if (StackedItems.StackedElements.Count == MaxStackSize)
                throw new InvalidOperationException($"Can't create more than {MaxStackSize} items in the StackedItem!");
            StackedItems.StackedElements.Add(element);
            return this;
        }
    }
}