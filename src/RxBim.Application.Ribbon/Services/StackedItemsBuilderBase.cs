namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Collections.Generic;
    using Abstractions;

    /// <inheritdoc />
    public abstract class StackedItemsBuilderBase<TButton> : IStackedItemsBuilder
        where TButton : IButtonBuilder
    {
        /// <summary>
        /// Items count
        /// </summary>
        public int ItemsCount => Buttons.Count;

        /// <summary>
        /// Buttons
        /// </summary>
        public IList<TButton> Buttons { get; } = new List<TButton>(3);

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        public IStackedItemsBuilder Button(string name, string text, Type externalCommandType, Action<IButtonBuilder> action = null)
        {
            if (Buttons.Count == 3)
            {
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");
            }

            var button = CreateButton(name, text, externalCommandType);
            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }

        /// <summary>
        /// Returns a new button
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        /// <param name="commandType">Button command class type</param>
        protected abstract TButton CreateButton(string name, string text, Type commandType);
    }
}