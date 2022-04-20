namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Abstractions.ConfigurationBuilders;
    using Models;
    using Models.Configurations;

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
        public IButtonBuilder SetLargeImage(string imageRelativePath, ThemeType theme = ThemeType.All)
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
        public IButtonBuilder SetSmallImage(string imageRelativePath, ThemeType theme = ThemeType.All)
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
        public IButtonBuilder SetDescription(string description)
        {
            BuildingButton.Description = description;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetToolTip(string toolTip)
        {
            BuildingButton.ToolTip = toolTip;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetText(string text)
        {
            BuildingButton.Text = text;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetHelpUrl(string url)
        {
            BuildingButton.HelpUrl = url;
            return this;
        }
    }
}