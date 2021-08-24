namespace RxBim.Application.Ui.Revit.Api.Models
{
    using System;
    using RxBim.Application.Ui.Api.Models;

    /// <summary>
    /// StackedItem
    /// </summary>
    public class StackedItem : StackedItemBase<Button>
    {
        /// <inheritdoc />
        protected override Button GetButton(string name, string text, Type externalCommandType)
        {
            return new (name, text, externalCommandType);
        }
    }
}