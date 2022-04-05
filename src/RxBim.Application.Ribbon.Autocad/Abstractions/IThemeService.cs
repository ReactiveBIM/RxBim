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
        /// Current theme changed event.
        /// </summary>
        event EventHandler ThemeChanged;

        /// <summary>
        /// Runs the service.
        /// </summary>
        void Run();

        /// <summary>
        /// Returns current theme type.
        /// </summary>
        ThemeType GetCurrentTheme();
    }
}