namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Applies images to a platform-specific ribbon button.
    /// </summary>
    /// <typeparam name="TButton">Platform-specific ribbon button type.</typeparam>
    public interface IRibbonButtonImageAdapter<in TButton>
        where TButton : class
    {
        /// <summary>
        /// Applies images to the button.
        /// </summary>
        /// <param name="button">Platform-specific ribbon button.</param>
        /// <param name="images">Button images.</param>
        void ApplyImages(TButton button, ButtonImages images);
    }
}