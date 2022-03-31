namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using Ribbon.Models;

    /// <summary>
    /// Theme service
    /// </summary>
    public interface IThemeService
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