namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    /// <summary>
    /// Base implementation of a button builder.
    /// </summary>
    public abstract class ButtonBuilder<TButton> : IButtonBuilder
        where TButton : Button, new()
    {
        /// <summary>
        /// Initializes a new instance of the button builder.
        /// </summary>
        /// <param name="name">The button name.</param>
        protected ButtonBuilder(string name)
        {
            BuildingButton.Name = name;
        }

        /// <summary>
        /// The button to create configuration.
        /// </summary>
        public TButton BuildingButton { get; } = new();

        /// <inheritdoc />
        public IButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All)
        {
            switch (theme)
            {
                case ThemeType.All:
                case ThemeType.Dark:
                    BuildingButton.LargeImage = imageRelativePath;
                    break;
                case ThemeType.Light:
                    BuildingButton.LargeImageLight = imageRelativePath;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SmallImage(string imageRelativePath, ThemeType theme = ThemeType.All)
        {
            switch (theme)
            {
                case ThemeType.All:
                case ThemeType.Dark:
                    BuildingButton.SmallImage = imageRelativePath;
                    break;
                case ThemeType.Light:
                    BuildingButton.SmallImageLight = imageRelativePath;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder Description(string description)
        {
            BuildingButton.Description = description;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder ToolTip(string toolTip)
        {
            BuildingButton.ToolTip = toolTip;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder Text(string text)
        {
            BuildingButton.Text = text;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder HelpUrl(string url)
        {
            BuildingButton.HelpUrl = url;
            return this;
        }
    }
}