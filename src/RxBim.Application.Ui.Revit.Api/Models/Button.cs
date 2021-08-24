namespace RxBim.Application.Ui.Revit.Api.Models
{
    using System;
    using Autodesk.Revit.UI;
    using RxBim.Application.Ui.Api.Models;

    /// <summary>
    /// Кнопка
    /// </summary>
    public class Button : ButtonBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="text">Текст</param>
        /// <param name="externalCommandType">Тип вызываемой команды</param>
        public Button(string name, string text, Type externalCommandType)
            : base(name, text, externalCommandType)
        {
        }

        /// <summary>
        /// Справка
        /// </summary>
        internal ContextualHelp ContextualHelp { get; set; }

        /// <summary>
        /// Возвращает ButtonData
        /// </summary>
        internal virtual ButtonData GetButtonData()
        {
            var pushButtonData = new PushButtonData(Name, Text, AssemblyLocation, ClassName);
            pushButtonData.AvailabilityClassName = ClassName;
            SetBaseProperties(pushButtonData);
            return pushButtonData;
        }

        /// <summary>
        /// Задаёт базовые свойства
        /// </summary>
        /// <param name="button">Кнопка</param>
        protected void SetBaseProperties(ButtonData button)
        {
            if (ToolTip != null)
            {
                button.ToolTip = ToolTip;
            }

            if (LargeImage != null)
            {
                button.LargeImage = LargeImage;
            }

            if (SmallImage != null)
            {
                button.Image = SmallImage;
            }

            if (Description != null)
            {
                button.LongDescription = Description;
            }

            if (ContextualHelp != null)
            {
                button.SetContextualHelp(ContextualHelp);
            }
        }

        /// <inheritdoc />
        protected override void SetHelpUrlInternal(string url)
        {
            ContextualHelp = new ContextualHelp(ContextualHelpType.Url, url);
        }
    }
}