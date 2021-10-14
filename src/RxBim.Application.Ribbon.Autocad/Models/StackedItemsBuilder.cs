namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using Ribbon.Services;

    /// <inheritdoc />
    public class StackedItemsBuilder : StackedItemsBuilder<ButtonBuilder>
    {
        /// <inheritdoc />
        protected override ButtonBuilder CreateButton(string name, string text, Type commandType)
        {
            return new ButtonBuilder(name, text, commandType);
        }
    }
}