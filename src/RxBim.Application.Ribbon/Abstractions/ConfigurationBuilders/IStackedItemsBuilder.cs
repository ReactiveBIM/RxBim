namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Stack of ribbon items
    /// </summary>
    public interface IStackedItemsBuilder
    {
        /// <summary>
        /// Creates a button on the stack
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="commandType">
        /// Class which implements command. This command will be execute when user push the button
        /// </param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns><see cref="IStackedItemsBuilder"/> object where button were created</returns>
        IStackedItemsBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null);

        /// <summary>
        /// Create pull down button on the stack
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns><see cref="IStackedItemsBuilder"/> object where button were created</returns>
        IStackedItemsBuilder AddPullDownButton(string name, Action<IPulldownButtonBuilder> action);

        /// <summary>
        /// Adds a element in the stack.
        /// </summary>
        /// <param name="element">Element.</param>
        IStackedItemsBuilder AddElement(IRibbonPanelElement element);
    }
}