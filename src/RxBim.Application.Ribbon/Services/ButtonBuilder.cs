namespace RxBim.Application.Ribbon.Services
{
    using Abstractions;
    using Models;

    /// <summary>
    /// Base implementation of a button builder
    /// </summary>
    public abstract class ButtonBuilder<TButton> : RibbonControlBuilder<TButton>, IButtonBuilder
        where TButton : Button, new()
    {
        /// <summary>
        /// Initializes a new instance of the button builder
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        protected ButtonBuilder(string name, string text)
        {
            Control.Name = name;
            Control.Text = text;
        }

        /// <inheritdoc />
        public IButtonBuilder SetLargeImage(string imageRelativePath, ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.All:
                    Control.LargeImage = imageRelativePath;
                    break;
                case ThemeType.Dark:
                    Control.LargeImageDark = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Control.LargeImageLight = imageRelativePath;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetSmallImage(string imageRelativePath, ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.All:
                    Control.SmallImage = imageRelativePath;
                    break;
                case ThemeType.Dark:
                    Control.SmallImageDark = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Control.SmallImageLight = imageRelativePath;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetDescription(string description)
        {
            Control.Description = description;
            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetToolTip(string toolTip)
        {
            Control.ToolTip = toolTip;
            return this;
        }
    }
}