﻿namespace RxBim.Application.Ribbon.Revit.Extensions
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