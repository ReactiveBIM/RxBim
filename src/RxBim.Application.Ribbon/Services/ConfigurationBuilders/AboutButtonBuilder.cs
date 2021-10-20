namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Models.Configurations;
    using Shared;

    /// <summary>
    /// About button builder
    /// </summary>
    public class AboutButtonBuilder : ButtonBuilder<AboutButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutButtonBuilder"/> class.
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        /// <param name="content">About window content</param>
        public AboutButtonBuilder(string name, string text, AboutBoxContent content)
            : base(name, text)
        {
            BuildingButton.Content = content;
        }
    }
}
