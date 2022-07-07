namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Autodesk.AutoCAD.ApplicationServices;
    using static AutocadMenuConstants;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <inheritdoc cref="IColorThemeService" />
    internal class ColorThemeService : IColorThemeService, IDisposable
    {
        /// <inheritdoc/>
        public event EventHandler? ThemeChanged;

        /// <inheritdoc />
        public void Run()
        {
            Application.SystemVariableChanged += ApplicationOnSystemVariableChanged;
        }

        /// <inheritdoc />
        public ThemeType GetCurrentTheme()
        {
            var themeTypeValue = (short)Application.GetSystemVariable(ColorThemeVariableName);
            return themeTypeValue == 0 ? ThemeType.Dark : ThemeType.Light;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Application.SystemVariableChanged -= ApplicationOnSystemVariableChanged;
        }

        private void ApplicationOnSystemVariableChanged(object sender, SystemVariableChangedEventArgs e)
        {
            if (e.Name.Equals(ColorThemeVariableName, StringComparison.OrdinalIgnoreCase))
                ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}