namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Autodesk.Revit.UI;
    using ColorThemeType = ThemeType;
#if !(RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023)
    using Autodesk.Revit.UI.Events;
#endif

    /// <inheritdoc cref="IColorThemeService" />
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

#if RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023
            _ = application;
#else
            application.ThemeChanged += OnThemeChanged;
#endif
            _isRunning = true;
        }

        /// <inheritdoc />
        public ColorThemeType GetCurrentTheme()
        {
#if RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023
            return ColorThemeType.Light;
#else
            return UIThemeManager.CurrentTheme is UITheme.Light ? ColorThemeType.Light : ColorThemeType.Dark;
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isRunning)
                return;

#if !(RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023)
            application.ThemeChanged -= OnThemeChanged;
#endif
            _isRunning = false;
        }

#if !(RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023)
        private void OnThemeChanged(object? sender, ThemeChangedEventArgs e)
        {
#if !(RVT2019 || RVT2020 || RVT2021 || RVT2022 || RVT2023 || RVT2024)
            if (e.ThemeChangedType is not ThemeType.UITheme)
                return;
#endif
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
#endif
    }
}