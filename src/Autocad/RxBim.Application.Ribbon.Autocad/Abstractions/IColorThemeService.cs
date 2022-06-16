namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Color theme service.
    /// </summary>
    public interface IColorThemeService
    {
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