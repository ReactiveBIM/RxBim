namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Color theme service.
    /// </summary>
    public interface IColorThemeService
    {
        /// <summary>
        /// Current theme changed event
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