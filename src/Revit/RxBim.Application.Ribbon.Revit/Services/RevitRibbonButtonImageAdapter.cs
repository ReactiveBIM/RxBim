namespace RxBim.Application.Ribbon.Services
{
    using Autodesk.Revit.UI;

    /// <inheritdoc />
    internal sealed class RevitRibbonButtonImageAdapter : IRibbonButtonImageAdapter<RibbonButton>
    {
        /// <inheritdoc />
        public void ApplyImages(RibbonButton button, ButtonImages images)
        {
            button.Image = images.Image;
            button.LargeImage = images.LargeImage;
        }
    }
}