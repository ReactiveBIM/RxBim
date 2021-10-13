namespace RxBim.Application.Ribbon.Revit.Models
{
    using Autodesk.Revit.UI;
    using Di;
    using Ribbon.Services;

    /// <summary>
    /// Кнопка о программе
    /// </summary>
    public class AboutButtonBuilder : AboutButtonBuilderBase
    {
        /// <inheritdoc />
        public AboutButtonBuilder(string name, string text, IContainer container)
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