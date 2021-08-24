namespace RxBim.Application.Ui.Revit.Api.Extensions
{
    using System;
    using Autodesk.Revit.UI;
    using RxBim.Application.Ui.Api.Abstractions;

    /// <summary>
    /// Расширения для сгруппированных элементов
    /// </summary>
    public static class StackedItemExtensions
    {
        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="stackedItem">Stacked item</param>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public static IStackedItem Button<TExternalCommandClass>(
            this IStackedItem stackedItem,
            string name,
            string text,
            Action<IButton> action = null)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);
            return stackedItem.Button(name, text, commandClassType, action);
        }
    }
}