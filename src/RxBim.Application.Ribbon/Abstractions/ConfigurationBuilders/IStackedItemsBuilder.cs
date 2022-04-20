namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Defines a stack of ribbon items.
    /// </summary>
    public interface IStackedItemsBuilder
    {
        /// <summary>
        /// Adds a new button to the stack.
        /// </summary>
        /// <param name="name">The internal button name.</param>
        /// <param name="commandType"> A class which implements command.
        /// This command will be execute when user push the button. </param>
        /// <param name="action">An additional action.</param>
        IStackedItemsBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null);

        /// <summary>
        /// Adds a new pull down button to the stack.
        /// </summary>
        /// <param name="name">The internal button name.</param>
        /// <param name="action">An additional action.</param>
        IStackedItemsBuilder AddPullDownButton(string name, Action<IPulldownButtonBuilder> action);
    }
}