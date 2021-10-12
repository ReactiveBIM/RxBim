namespace RxBim.Application.Ribbon.Models
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Abstractions;

    /// <summary>
    /// Base implementation of a ribbon button
    /// </summary>
    public abstract class ButtonBase : IButton
    {
        private readonly Type _commandType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonBase"/> class.
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        /// <param name="commandType">Button command class type</param>
        protected ButtonBase(string name, string text, Type commandType)
        {
            Name = name;
            Text = text;
            _commandType = commandType;

            if (commandType != null)
            {
                CommandClassTypeName = commandType.FullName;
                CommandAssemblyLocation = commandType.Assembly.Location;
            }
        }

        /// <summary>
        /// Button command class type full name
        /// </summary>
        protected string CommandClassTypeName { get; }

        /// <summary>
        /// The path to the assembly that contains the command class
        /// </summary>
        protected string CommandAssemblyLocation { get; }

        /// <summary>
        /// Button name
        /// </summary>
        protected string Name { get; set; }

        /// <summary>
        /// Button label name
        /// </summary>
        protected string Text { get; set; }

        /// <summary>
        /// Button tooltip
        /// </summary>
        protected string ToolTip { get; set; }

        /// <summary>
        /// Large image for the button
        /// </summary>
        protected ImageSource LargeImage { get; set; }

        /// <summary>
        /// Small image for the button
        /// </summary>
        protected ImageSource SmallImage { get; set; }

        /// <summary>
        /// Button description
        /// </summary>
        protected string Description { get; set; }

        /// <inheritdoc />
        public virtual IButton SetToolTip(string toolTip, bool addVersion = true, string versionInfoPrefix = "")
        {
            ToolTip = toolTip;
            if (_commandType != null
                && addVersion)
            {
                if (!string.IsNullOrEmpty(toolTip))
                    ToolTip += Environment.NewLine;
                ToolTip += $"{versionInfoPrefix}{_commandType.Assembly.GetName().Version}";
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
        public IButton SetDescription(string description)
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