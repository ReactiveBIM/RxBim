namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Defines a builder for PulldownButton.
    /// </summary>
    public interface IPulldownButtonBuilder : IButtonBuilderBase<PullDownButton, IPulldownButtonBuilder>
    {
        /// <summary>
        /// Adds a new button the pulldown button.
        /// </summary>
        /// <param name="name">The button internal name.</param>
        /// <param name="commandType"> A class which implements command.
        /// This command will be execute when user push the button. </param>
        /// <param name="builder">The button builder.</param>
        IPulldownButtonBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommandButtonBuilder>? builder = null);
    }
}