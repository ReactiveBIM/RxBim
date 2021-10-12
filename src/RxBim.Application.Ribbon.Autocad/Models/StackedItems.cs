namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using Application.Ribbon.Models;

    /// <inheritdoc />
    public class StackedItems : StackedItemsBase<Button>
    {
        /// <inheritdoc />
        protected override Button CreateButton(string name, string text, Type commandType)
        {
            return new (name, text, commandType);
        }
    }
}