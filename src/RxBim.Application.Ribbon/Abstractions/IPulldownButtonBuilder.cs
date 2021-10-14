namespace RxBim.Application.Ribbon.Abstractions
{
    using System;

    /// <summary>
    /// PulldownButton
    /// </summary>
    public interface IPulldownButtonBuilder : IButtonBuilder
    {
        /// <summary>
        /// Create push button and add to the pulldown buttons
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="commandType">
        /// Class which implements command. This command will be execute when user push the button
        /// </param>
        IPulldownButtonBuilder AddCommandButton(string name, string text, Type commandType);

        /// <summary>
        /// Create push button and add to the pulldown buttons
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="commandType">
        /// Class which implements command. This command will be execute when user push the button
        /// </param>
        /// <param name="action">Additional action with whe button</param>
        IPulldownButtonBuilder AddCommandButton(
            string name,
            string text,
            Type commandType,
            Action<IButtonBuilder> action);
    }
}