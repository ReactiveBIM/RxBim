namespace PikTools.Application.Ui.Api.Builder
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Кнопка
    /// </summary>
    public class Button
    {
        private readonly string _className;
        private readonly string _assemblyLocation;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="text">текст</param>
        /// <param name="externalCommandType">тип вызываемой комманды</param>
        public Button(string name, string text, Type externalCommandType)
        {
            Name = name;
            Text = text;

            if (externalCommandType != null)
            {
                _className = externalCommandType.FullName;
                _assemblyLocation = externalCommandType.Assembly.Location;
            }
        }

        /// <summary>
        /// Имя кнопки
        /// </summary>
        protected string Name { get; private set; }

        /// <summary>
        /// Текст кнопки
        /// </summary>
        protected string Text { get; private set; }

        /// <summary>
        /// Большое изображение
        /// </summary>
        protected ImageSource LargeImage { get; private set; }

        /// <summary>
        /// Малое изображение
        /// </summary>
        protected ImageSource SmallImage { get; private set; }

        /// <summary>
        /// Описание
        /// </summary>
        protected string Description { get; private set; }

        /// <summary>
        /// Справка
        /// </summary>
        protected ContextualHelp ContextualHelp { get; private set; }

        /// <summary>
        /// Устанавливает большое изображение
        /// </summary>
        /// <param name="imageUri">imageUri</param>
        public Button SetLargeImage(Uri imageUri)
        {
            if (imageUri != null)
            {
                LargeImage = new BitmapImage(imageUri);
            }

            return this;
        }

        /// <summary>
        /// Устанавливает маленькое изображение
        /// </summary>
        /// <param name="imageUri">изображение</param>
        public Button SetSmallImage(Uri imageUri)
        {
            if (imageUri != null)
            {
                SmallImage = new BitmapImage(imageUri);
            }

            return this;
        }

        /// <summary>
        /// Устанавливает опиание
        /// </summary>
        /// <param name="description">описание</param>
        public Button SetLongDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Устанавливает справку
        /// </summary>
        /// <param name="contextualHelpType">српавка</param>
        /// <param name="helpPath">путь к справке</param>
        public Button SetContextualHelp(ContextualHelpType contextualHelpType, string helpPath)
        {
            ContextualHelp = new ContextualHelp(contextualHelpType, helpPath);
            return this;
        }

        /// <summary>
        /// Устанавливает url справки
        /// </summary>
        /// <param name="url">url</param>
        public Button SetHelpUrl(string url)
        {
            ContextualHelp = new ContextualHelp(ContextualHelpType.Url, url);
            return this;
        }

        /// <summary>
        /// Заканчивает настройку кнопки
        /// </summary>
        internal virtual ButtonData Finish()
        {
            PushButtonData pushButtonData = new PushButtonData(Name, Text, _assemblyLocation, _className);

            if (LargeImage != null)
            {
                pushButtonData.LargeImage = LargeImage;
            }

            if (SmallImage != null)
            {
                pushButtonData.Image = SmallImage;
            }

            if (Description != null)
            {
                pushButtonData.LongDescription = Description;
            }

            if (ContextualHelp != null)
            {
                pushButtonData.SetContextualHelp(ContextualHelp);
            }

            return pushButtonData;
        }
    }
}