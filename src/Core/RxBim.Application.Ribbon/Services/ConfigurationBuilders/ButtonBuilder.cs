namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    /// <summary>
    /// Base implementation of a button builder.
    /// </summary>
    public abstract class ButtonBuilder<TButton, TButtonBuilder> : RibbonPanelItemBuilderBase<TButton, TButtonBuilder>, IButtonBuilder<TButtonBuilder>
        where TButton : Button, new()
        where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
    {
        /// <inheritdoc />
        protected ButtonBuilder(string name)
            : base(name)
        {
        }

        /// <inheritdoc />
        public TButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All)
        {
            switch (theme)
            {
                case ThemeType.All:
                case ThemeType.Dark:
                    Item.LargeImage = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Item.LargeImageLight = imageRelativePath;
                    break;
            }

            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public override TButtonBuilder Image(string imageRelativePath, ThemeType theme = ThemeType.All)
        {
            switch (theme)
            {
                case ThemeType.All:
                case ThemeType.Dark:
                    Item.Image = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Item.ImageLight = imageRelativePath;
                    break;
            }

            return (this as TButtonBuilder)!;
        }

        /// <inheritdoc />
        public TButtonBuilder HelpUrl(string url)
        {
            Item.HelpUrl = url;
            return (this as TButtonBuilder)!;
        }
    }
}