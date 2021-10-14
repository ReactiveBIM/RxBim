namespace RxBim.Application.Ribbon.Services
{
    using Abstractions;
    using Models;

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
            Button = new TButton
            {
                Name = name,
                Text = text
            };
        }

        /// <summary>
        /// Building button
        /// </summary>
        protected TButton Button { get; }

        /// <inheritdoc />
        public IButtonBuilder SetLargeImage(string imageRelativePath, ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.All:
                    Button.LargeImage = imageRelativePath;
                    break;
                case ThemeType.Dark:
                    Button.LargeImageDark = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Button.LargeImageLight = imageRelativePath;
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
                    Button.SmallImage = imageRelativePath;
                    break;
                case ThemeType.Dark:
                    Button.SmallImageDark = imageRelativePath;
                    break;
                case ThemeType.Light:
                    Button.SmallImageLight = imageRelativePath;
                    break;
            }

            return this;
        }

        /// <inheritdoc />
        public IButtonBuilder SetDescription(string description)
        {
            Button.Description = description;
            return this;
        }
    }
}