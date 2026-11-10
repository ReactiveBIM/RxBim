namespace RxBim.Application.Ribbon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <inheritdoc />
    public class ThemedRibbonButtonService<TButton>(
        IColorThemeService colorThemeService,
        IButtonImageProvider imageProvider,
        IRibbonButtonImageAdapter<TButton> imageAdapter)
        : IThemedRibbonButtonService<TButton>
        where TButton : class
    {
        private readonly List<(TButton Button, Button Config)> _buttons = new();
        private bool _isRunning;

        /// <inheritdoc />
        public void Run()
        {
            if (_isRunning)
                return;

            colorThemeService.ThemeChanged += OnThemeChanged;
            try
            {
                colorThemeService.Run();
                _isRunning = true;
            }
            catch
            {
                colorThemeService.ThemeChanged -= OnThemeChanged;
                throw;
            }
        }

        /// <inheritdoc />
        public void Register(TButton button, Button buttonConfig)
        {
            if (_buttons.Any(x => ReferenceEquals(x.Button, button)))
                return;

            _buttons.Add((button, buttonConfig));
            ApplyImages(button, buttonConfig);
        }

        /// <inheritdoc />
        public void ApplyCurrentTheme()
        {
            _buttons.ForEach(x => ApplyImages(x.Button, x.Config));
        }

        /// <inheritdoc />
        public void Clear()
        {
            _buttons.Clear();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isRunning)
                return;

            colorThemeService.ThemeChanged -= OnThemeChanged;
            _isRunning = false;
        }

        private void ApplyImages(TButton button, Button buttonConfig)
        {
            imageAdapter.ApplyImages(button, imageProvider.GetImages(buttonConfig));
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            ApplyCurrentTheme();
        }
    }
}