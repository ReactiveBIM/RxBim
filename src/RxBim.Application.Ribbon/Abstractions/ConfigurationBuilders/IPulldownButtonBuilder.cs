namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
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
        /// <param name="commandType">
        /// Class which implements command. This command will be execute when user push the button
        /// </param>
        /// <param name="action">Additional action with whe button</param>
        IPulldownButtonBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null);
    }
}