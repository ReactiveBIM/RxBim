namespace RxBim.Application.Ribbon.Revit.Extensions
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Models;

    /// <summary>
    /// Расширения для кнопки
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Устанавливает справку
        /// </summary>
        /// <param name="buttonBuilder">Кнопка</param>
        /// <param name="contextualHelpType">Справка</param>
        /// <param name="helpPath">путь к справке</param>
        public static IButtonBuilder SetContextualHelp(
            this IButtonBuilder buttonBuilder,
            ContextualHelpType contextualHelpType,
            string helpPath)
        {
            if (buttonBuilder is ButtonBuilder revitButton)
            {
                revitButton.ContextualHelp = new ContextualHelp(contextualHelpType, helpPath);
            }

            return buttonBuilder;
        }
    }
}