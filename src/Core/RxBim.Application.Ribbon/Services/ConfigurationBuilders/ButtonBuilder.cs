namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    /// <summary>
    /// Base implementation of a button builder.
    /// </summary>
    public abstract class ButtonBuilder<TButton, TButtonBuilder> : IButtonBuilder<TButtonBuilder>
        where TButton : Button, new()
        where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the button builder.
        /// </summary>
        /// <param name="name">The button name.</param>
        protected ButtonBuilder(string name)
        {
            Button.Name = name;
        }

        /// <summary>
        /// The button to create configuration.
        /// </summary>
        protected TButton Button { get; } = new();

        /// <inheritdoc />
        public TButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All)
        {
            switch (theme)
            {
                case ThemeType.All:
                case ThemeType.Dark:
                    Button.LargeImage = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Button.LargeImageLight = imageRelativePath;
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
                    Button.SmallImage = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Button.SmallImageLight = imageRelativePath;
                    break;
            }

            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder Description(string description)
        {
            Button.Description = description;
            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder ToolTip(string toolTip)
        {
            Button.ToolTip = toolTip;
            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder Text(string text)
        {
            Button.Text = text;
            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder HelpUrl(string url)
        {
            Button.HelpUrl = url;
            return (this as TButtonBuilder)!;
        }

        /// <summary>
        /// Returns button.
        /// </summary>
        internal TButton Build()
        {
            return Button;
        }
    }
}