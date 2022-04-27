namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    /// <summary>
    /// About button builder.
    /// </summary>
    public class AboutButtonBuilder : ButtonBuilder<AboutButton, IAboutButtonBuilder>, IAboutButtonBuilder
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
