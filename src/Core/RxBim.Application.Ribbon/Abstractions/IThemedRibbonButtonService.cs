namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Stores platform-specific ribbon buttons and updates their images when the color theme changes.
    /// </summary>
    /// <typeparam name="TButton">Platform-specific ribbon button type.</typeparam>
    public interface IThemedRibbonButtonService<in TButton> : IDisposable
        where TButton : class
    {
        /// <summary>
        /// Starts tracking color theme changes.
        /// </summary>
        void Run();

        /// <summary>
        /// Registers a created ribbon button.
        /// </summary>
        /// <param name="button">Platform-specific ribbon button.</param>
        /// <param name="buttonConfig">Button configuration.</param>
        void Register(TButton button, Button buttonConfig);

        /// <summary>
        /// Applies the current color theme to all registered buttons.
        /// </summary>
        void ApplyCurrentTheme();

        /// <summary>
        /// Clears registered ribbon buttons.
        /// </summary>
        void Clear();
    }
}