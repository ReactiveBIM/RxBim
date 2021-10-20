namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Stack of ribbon items
    /// </summary>
    public interface IStackedItemsBuilder
    {
        /// <summary>
        /// Items count is correct
        /// </summary>
        public bool HasCorrectItemsCount { get; }

        /// <summary>
        /// Creates a button on the stack
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="commandType">
        /// Class which implements command. This command will be execute when user push the button
        /// </param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns><see cref="IStackedItemsBuilder"/> object where button were created</returns>
        IStackedItemsBuilder AddCommandButton(
            string name,
            string text,
            Type commandType,
            Action<ICommandButtonBuilder>? action = null);
    }
}