namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Models;

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

            StackedItems.Buttons.Add(buttonBuilder.Button);

            return this;
        }
    }
}