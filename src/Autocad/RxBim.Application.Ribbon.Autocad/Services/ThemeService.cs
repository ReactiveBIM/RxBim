namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Autodesk.AutoCAD.ApplicationServices;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <inheritdoc cref="IThemeService" />
    public class ThemeService : IThemeService, IDisposable
    {
        private const string ThemeVariableName = "COLORTHEME";

        private readonly IButtonService _buttonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeService"/> class.
        /// </summary>
        /// <param name="buttonService"><see cref="IButtonService"/>.</param>
        public ThemeService(IButtonService buttonService)
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
            var themeTypeValue = (short)Application.GetSystemVariable(ThemeVariableName);
            return themeTypeValue == 0 ? ThemeType.Dark : ThemeType.Light;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Application.SystemVariableChanged -= ApplicationOnSystemVariableChanged;
        }

        private void ApplicationOnSystemVariableChanged(object sender, SystemVariableChangedEventArgs e)
        {
            if (e.Name.Equals(ThemeVariableName, StringComparison.OrdinalIgnoreCase))
                _buttonService.ApplyCurrentTheme();
        }
    }
}