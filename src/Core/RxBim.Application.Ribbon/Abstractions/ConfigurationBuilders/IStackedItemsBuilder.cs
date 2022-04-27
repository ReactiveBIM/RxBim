namespace RxBim.Application.Ribbon
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
        /// <param name="name">The button internal name.</param>
        /// <param name="commandType"> A class which implements command.
        ///     This command will be execute when user push the button. </param>
        /// <param name="builder">The button builder.</param>
        IStackedItemsBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommnadButtonBuilder>? builder = null);

        /// <summary>
        /// Adds a new pull down button to the stack.
        /// </summary>
        /// <param name="name">The button internal name.</param>
        /// <param name="builder">The pull-down button builder.</param>
        IStackedItemsBuilder PullDownButton(string name, Action<IPulldownButtonBuilder> builder);
    }
}