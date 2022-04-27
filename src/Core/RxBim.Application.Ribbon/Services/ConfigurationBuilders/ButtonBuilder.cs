namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    /// <summary>
    /// Base implementation of a button builder.
    /// </summary>
    public abstract class ButtonBuilder<TButton, TButtonBuilder> : IButtonBuilderBase<TButton, TButtonBuilder>
        where TButton : Button, new()
        where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
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
        public TButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All)
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

            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder SmallImage(string imageRelativePath, ThemeType theme = ThemeType.All)
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

            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder Description(string description)
        {
            BuildingButton.Description = description;
            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder ToolTip(string toolTip)
        {
            BuildingButton.ToolTip = toolTip;
            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder Text(string text)
        {
            BuildingButton.Text = text;
            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder HelpUrl(string url)
        {
            BuildingButton.HelpUrl = url;
            return (this as TButtonBuilder)!;
        }
    }
}