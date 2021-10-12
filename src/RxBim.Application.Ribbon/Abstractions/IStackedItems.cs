namespace RxBim.Application.Ribbon.Abstractions
{
    using System;

    /// <summary>
    /// Stack of ribbon items
    /// </summary>
    public interface IStackedItems
    {
        /// <summary>
        /// Creates a button on the stack
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns><see cref="IStackedItems"/> object where button were created</returns>
        IStackedItems Button(string name, string text, Type externalCommandType, Action<IButton> action = null);
    }
}