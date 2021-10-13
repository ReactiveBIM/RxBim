namespace RxBim.Application.Ribbon.Abstractions
{
    using System;

    /// <summary>
    /// PulldownButton
    /// </summary>
    public interface IPulldownButtonBuilder : IButtonBuilder
    {
        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        IPulldownButtonBuilder Button(string name, string text, Type externalCommandType);

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPulldownButtonBuilder Button(string name, string text, Type externalCommandType, Action<IButtonBuilder> action);
    }
}