namespace RxBim.Application.Ribbon.Autocad.Models
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;
    using Ribbon.Services;

    /// <inheritdoc />
    public class AboutButtonBuilder : Ribbon.Services.ConfigurationBuilders.AboutButtonBuilder
    {
        /// <inheritdoc />
        public AboutButtonBuilder(string name, string text, IContainer container)
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