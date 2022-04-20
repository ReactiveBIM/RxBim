namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using System;
    using Models;

    /// <summary>
    /// Theme service.
    /// </summary>
    public interface IThemeService
    {
        /// <summary>
        /// Raises when UI theme is changed.
        /// </summary>
        event EventHandler ThemeChanged;

        /// <summary>
        /// Starts the service.
        /// </summary>
        void Run();

        /// <summary>
        /// Returns current theme.
        /// </summary>
        ThemeType GetCurrentTheme();
    }
}