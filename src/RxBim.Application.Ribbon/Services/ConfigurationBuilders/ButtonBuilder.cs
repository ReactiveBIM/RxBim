namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Abstractions.ConfigurationBuilders;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// Base implementation of a button builder
    /// </summary>
    public abstract class ButtonBuilder<TButton> : IButtonBuilder
        where TButton : Button, new()
    {
        /// <summary>
        /// Initializes a new instance of the button builder
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        protected ButtonBuilder(string name, string text)
        {
            BuildingButton.Name = name;
            BuildingButton.Text = text;
        }

        /// <summary>
        /// Building ribbon control
        /// </summary>
        public TButton BuildingButton { get; } = new ();

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
    }
}