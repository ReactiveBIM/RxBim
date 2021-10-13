namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Ribbon.Services;

    /// <summary>
    /// StackedItem
    /// </summary>
    public class StackedItemsBuilder : StackedItemsBuilderBase<ButtonBuilder>
    {
        /// <inheritdoc />
        protected override ButtonBuilder CreateButton(string name, string text, Type commandType)
        {
            return new ButtonBuilder(name, text, commandType);
        }
    }
}