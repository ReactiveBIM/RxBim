namespace RxBim.Application.Ribbon.Autocad.Models
{
    using Application.Ribbon.Models;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;

    /// <inheritdoc />
    public class AboutButton : AboutButtonBase
    {
        /// <inheritdoc />
        public AboutButton(string name, string text, IContainer container)
            : base(name, text, container)
        {
        }

        /// <inheritdoc />
        protected override void ShowContentInStandardMessageBox()
        {
            var message = Content?.ToString();
            if (!string.IsNullOrWhiteSpace(message))
            {
                Application.ShowAlertDialog(message);
            }
        }
    }
}