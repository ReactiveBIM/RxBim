namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Autodesk.Revit.UI;
    using ColorThemeType = RxBim.Application.Ribbon.ThemeType;
#if RVT_HAS_UI_THEME
    using Autodesk.Revit.UI.Events;
#endif

    /// <inheritdoc />
    internal sealed class RevitColorThemeService(UIControlledApplication application)
        : IColorThemeService, IDisposable
    {
        private bool _isRunning;

        /// <inheritdoc />
        public event EventHandler? ThemeChanged;

        /// <inheritdoc />
        public void Run()
        {
            if (_isRunning)
                return;

#if RVT_HAS_UI_THEME
            application.ThemeChanged += OnThemeChanged;
#else
            _ = application;
#endif
            _isRunning = true;
        }

        /// <inheritdoc />
        public ColorThemeType GetCurrentTheme()
        {
#if RVT_HAS_UI_THEME
            return UIThemeManager.CurrentTheme is UITheme.Light ? ColorThemeType.Light : ColorThemeType.Dark;
#else
            return ColorThemeType.Light;
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isRunning)
                return;

#if RVT_HAS_UI_THEME
            application.ThemeChanged -= OnThemeChanged;
#endif
            _isRunning = false;
        }

#if RVT_HAS_UI_THEME
        private void OnThemeChanged(object? sender, ThemeChangedEventArgs e)
        {
#if RVT_HAS_THEME_CHANGED_TYPE
            if (e.ThemeChangedType is not Autodesk.Revit.UI.ThemeType.UITheme)
                return;
#endif
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
#endif
    }
}