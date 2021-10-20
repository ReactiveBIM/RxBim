namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

    /// <inheritdoc />
    public class StackedItemsBuilder : IStackedItemsBuilder
    {
        /// <summary>
        /// Builds StackedItems
        /// </summary>
        public StackedItems StackedItems { get; } = new ();

        /// <inheritdoc />
        public bool HasCorrectItemsCount => StackedItems.Buttons.Count is >= 2 and <= 3;

        /// <inheritdoc />
        public IStackedItemsBuilder AddCommandButton(
            string name,
            string text,
            Type commandType,
            Action<ICommandButtonBuilder>? action = null)
        {
            if (StackedItems.Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            var buttonBuilder = new CommandButtonBuilder(name, text, commandType);
            action?.Invoke(buttonBuilder);

            StackedItems.Buttons.Add(buttonBuilder.BuildingButton);

            return this;
        }
    }
}