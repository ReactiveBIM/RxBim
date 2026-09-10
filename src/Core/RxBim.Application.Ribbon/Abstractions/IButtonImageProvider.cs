namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Provides button images for the current color theme.
    /// </summary>
    public interface IButtonImageProvider
    {
        /// <summary>
        /// Returns button images for the current color theme.
        /// </summary>
        /// <param name="buttonConfig">Button configuration.</param>
        ButtonImages GetImages(Button buttonConfig);
    }
}