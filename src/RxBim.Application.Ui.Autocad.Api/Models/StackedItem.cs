namespace RxBim.Application.Ui.Autocad.Api.Models
{
    using System;
    using RxBim.Application.Ui.Api.Models;

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