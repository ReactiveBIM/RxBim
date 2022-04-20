namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Models.Configurations;
    using Shared;

    /// <summary>
    /// About button builder.
    /// </summary>
    public class AboutButtonBuilder : ButtonBuilder<AboutButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutButtonBuilder"/> class.
        /// </summary>
        /// <param name="name">The button name.</param>
        /// <param name="content">The About window content.</param>
        public AboutButtonBuilder(string name, AboutBoxContent content)
            : base(name)
        {
            BuildingButton.Content = content;
        }
    }
}
