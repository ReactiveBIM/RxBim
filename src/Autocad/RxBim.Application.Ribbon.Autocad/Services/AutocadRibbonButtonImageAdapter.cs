namespace RxBim.Application.Ribbon.Services
{
    using Autodesk.Windows;

    /// <inheritdoc />
    internal sealed class AutocadRibbonButtonImageAdapter : IRibbonButtonImageAdapter<RibbonButton>
    {
        /// <inheritdoc />
        public void ApplyImages(RibbonButton button, ButtonImages images)
        {
            button.Image = images.Image;
            button.LargeImage = images.LargeImage;
        }
    }
}