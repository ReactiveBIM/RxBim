namespace RxBim.Application.Ui.Api.Models
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Abstractions;

    /// <summary>
    /// Базовый класс кнопки
    /// </summary>
    public abstract class ButtonBase : IButton
    {
        private readonly Type _externalCommandType;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="text">Текст</param>
        /// <param name="externalCommandType">Тип вызываемой команды</param>
        protected ButtonBase(string name, string text, Type externalCommandType)
        {
            Name = name;
            Text = text;
            _externalCommandType = externalCommandType;

            if (externalCommandType != null)
            {
                ClassName = externalCommandType.FullName;
                AssemblyLocation = externalCommandType.Assembly.Location;
            }
        }

        /// <summary>
        /// Полное название типа командного класса
        /// </summary>
        protected string ClassName { get; }

        /// <summary>
        /// Путь к сборке с командным классом
        /// </summary>
        protected string AssemblyLocation { get; }

        /// <summary>
        /// Имя кнопки
        /// </summary>
        protected string Name { get; set; }

        /// <summary>
        /// Текст кнопки
        /// </summary>
        protected string Text { get; set; }

        /// <summary>
        /// Всплывающий текст с описанием кнопки
        /// </summary>
        protected string ToolTip { get; set; }

        /// <summary>
        /// Большое изображение
        /// </summary>
        protected ImageSource LargeImage { get; set; }

        /// <summary>
        /// Малое изображение
        /// </summary>
        protected ImageSource SmallImage { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        protected string Description { get; set; }

        /// <inheritdoc />
        public virtual IButton SetToolTip(string toolTip, bool addVersion = true)
        {
            ToolTip = toolTip;
            if (_externalCommandType != null
                && addVersion)
            {
                if (!string.IsNullOrEmpty(toolTip))
                    ToolTip += Environment.NewLine;
                ToolTip += $"Версия: {_externalCommandType.Assembly.GetName().Version}";
            }

            return this;
        }

        /// <inheritdoc />
        public IButton SetLargeImage(Uri imageUri)
        {
            if (imageUri != null)
            {
                LargeImage = new BitmapImage(imageUri);
            }

            return this;
        }

        /// <inheritdoc />
        public IButton SetSmallImage(Uri imageUri)
        {
            if (imageUri != null)
            {
                SmallImage = new BitmapImage(imageUri);
            }

            return this;
        }

        /// <inheritdoc />
        public IButton SetLongDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <inheritdoc />
        public IButton SetHelpUrl(string url)
        {
            SetHelpUrlInternal(url);
            return this;
        }

        /// <summary>
        /// Set URL for help
        /// </summary>
        /// <param name="url">Url</param>
        protected abstract void SetHelpUrlInternal(string url);
    }
}