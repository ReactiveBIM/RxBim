namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using Models;

    /// <summary>
    /// Theme service
    /// </summary>
    public interface IColorThemeService
    {
        /// <summary>
        /// Runs the service
        /// </summary>
        void Run();

        /// <summary>
        /// Returns current theme type
        /// </summary>
        ThemeType GetCurrentTheme();
    }
}