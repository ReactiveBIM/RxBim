namespace RxBim.Application.Ui.Autocad.Api.Models
{
    using Di;
    using RxBim.Application.Ui.Api.Models;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

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