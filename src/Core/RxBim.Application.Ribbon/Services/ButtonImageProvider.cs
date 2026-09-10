namespace RxBim.Application.Ribbon
{
    using System.Reflection;

    /// <inheritdoc />
    public class ButtonImageProvider(MenuData menuData, IColorThemeService colorThemeService)
        : IButtonImageProvider
    {
        /// <inheritdoc />
        public ButtonImages GetImages(Button buttonConfig)
        {
            var assembly = GetButtonAssembly(buttonConfig);
            var themeType = colorThemeService.GetCurrentTheme();
            var image = menuData.GetIconImage(buttonConfig.ResolveImagePath(themeType), assembly);
            var largeImage = menuData.GetIconImage(buttonConfig.ResolveLargeImagePath(themeType), assembly);
            return new ButtonImages(image, largeImage);
        }

        private Assembly? GetButtonAssembly(Button buttonConfig)
        {
            var commandTypeName = buttonConfig switch
            {
                CommandButton commandButton => commandButton.CommandType,
                ToggleCommandButton toggleCommandButton => toggleCommandButton.CommandType,
                _ => null
            };

            return !string.IsNullOrWhiteSpace(commandTypeName)
                ? menuData.MenuAssembly.GetTypeByName(commandTypeName!).Assembly
                : null;
        }
    }
}