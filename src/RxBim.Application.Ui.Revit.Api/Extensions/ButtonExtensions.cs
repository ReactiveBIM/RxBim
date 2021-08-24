namespace RxBim.Application.Ui.Revit.Api.Extensions
{
    using Autodesk.Revit.UI;
    using Models;
    using RxBim.Application.Ui.Api.Abstractions;

    /// <summary>
    /// Расширения для кнопки
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Устанавливает справку
        /// </summary>
        /// <param name="button">Кнопка</param>
        /// <param name="contextualHelpType">Справка</param>
        /// <param name="helpPath">путь к справке</param>
        public static IButton SetContextualHelp(
            this IButton button,
            ContextualHelpType contextualHelpType,
            string helpPath)
        {
            if (button is Button revitButton)
            {
                revitButton.ContextualHelp = new ContextualHelp(contextualHelpType, helpPath);
            }

            return button;
        }
    }
}