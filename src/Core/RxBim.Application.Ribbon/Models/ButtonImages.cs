namespace RxBim.Application.Ribbon
{
    using System.Windows.Media;

    /// <summary>
    /// Images displayed by a ribbon button.
    /// </summary>
    public class ButtonImages(ImageSource? image, ImageSource? largeImage)
    {
        /// <summary>
        /// Gets the standard button image.
        /// </summary>
        public ImageSource? Image { get; } = image;

        /// <summary>
        /// Gets the large button image.
        /// </summary>
        public ImageSource? LargeImage { get; } = largeImage;
    }
}