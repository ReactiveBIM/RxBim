namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Theme service.
    /// </summary>
    public interface IThemeService
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