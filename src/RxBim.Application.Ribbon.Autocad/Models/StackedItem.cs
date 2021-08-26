namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using Application.Ribbon.Models;

    /// <inheritdoc />
    public class StackedItem : StackedItemBase<Button>
    {
        /// <inheritdoc />
        protected override Button GetButton(string name, string text, Type externalCommandType)
        {
            return new (name, text, externalCommandType);
        }
    }
}