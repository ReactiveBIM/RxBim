namespace RxBim.Application.Ribbon.Revit.Models
{
    using Application.Ribbon.Models;
    using Autodesk.Revit.UI;
    using Di;

    /// <summary>
    /// Кнопка о программе
    /// </summary>
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
            TaskDialog.Show(Name, Content?.ToString());
        }
    }
}