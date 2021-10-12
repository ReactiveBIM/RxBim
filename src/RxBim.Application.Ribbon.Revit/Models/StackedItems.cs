namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Application.Ribbon.Models;

    /// <summary>
    /// StackedItem
    /// </summary>
    public class StackedItems : StackedItemsBase<Button>
    {
        /// <inheritdoc />
        protected override Button CreateButton(string name, string text, Type commandType)
        {
            return new Button(name, text, commandType);
        }
    }
}