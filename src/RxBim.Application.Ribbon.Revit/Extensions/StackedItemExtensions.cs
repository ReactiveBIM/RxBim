namespace RxBim.Application.Ribbon.Revit.Extensions
{
    using System;
    using Abstractions;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Расширения для сгруппированных элементов
    /// </summary>
    public static class StackedItemExtensions
    {
        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="stackedItemsBuilder">Stacked item</param>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public static IStackedItemsBuilder Button<TExternalCommandClass>(
            this IStackedItemsBuilder stackedItemsBuilder,
            string name,
            string text,
            Action<IButtonBuilder> action = null)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);
            return stackedItemsBuilder.Button(name, text, commandClassType, action);
        }
    }
}