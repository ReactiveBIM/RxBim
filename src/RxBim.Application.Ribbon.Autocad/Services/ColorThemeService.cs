namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices;
    using Models;
    using static AutocadMenuConstants;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <inheritdoc cref="IColorThemeService" />
    public class ColorThemeService : IColorThemeService, IDisposable
    {
        private readonly IButtonService _buttonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorThemeService"/> class.
        /// </summary>
        /// <param name="buttonService"><see cref="IButtonService"/>.</param>
        public ColorThemeService(IButtonService buttonService)
        {
            _buttonService = buttonService;
        }

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
                _buttonService.ApplyCurrentTheme();
        }
    }
}