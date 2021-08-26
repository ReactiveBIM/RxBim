namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Application.Ribbon.Models;

    /// <summary>
    /// StackedItem
    /// </summary>
    public class StackedItem : StackedItemBase<Button>
    {
        /// <inheritdoc />
        protected override Button GetButton(string name, string text, Type externalCommandType)
        {
            return new Button(name, text, externalCommandType);
        }
    }
}