namespace RxBim.Application.Ribbon.Models.Configurations
{
    using Shared;

    /// <summary>
    /// Button for displaying the About window
    /// </summary>
    public class AboutButton : Button
    {
        /// <summary>
        /// About window content
        /// </summary>
        public AboutBoxContent Content { get; set; } = new ();
    }
}