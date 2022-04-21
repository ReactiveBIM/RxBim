namespace RxBim.Application.Ribbon
{
    using RxBim.Shared;

    /// <summary>
    /// Button for displaying the About window.
    /// </summary>
    public class AboutButton : Button
    {
        /// <summary>
        /// About window content.
        /// </summary>
        public AboutBoxContent Content { get; set; } = new();
    }
}