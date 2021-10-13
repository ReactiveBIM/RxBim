namespace RxBim.Application.Ribbon.Models
{
    using System;
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
        /// <param name="commandType">Button command class type</param>
        protected ButtonBase(Type commandType)
        {
            // Name = name;
            // Text = text;
            _commandType = commandType;

            // if (commandType != null)
            // {
            //     CommandClassTypeName = commandType.FullName;
            //     CommandAssemblyLocation = commandType.Assembly.Location;
            // }
        }

        /// <inheritdoc />
        public virtual IButton SetToolTip(string toolTip, bool addVersion = true, string versionHeader = "")
        {
            if (_commandType != null && addVersion)
            {
                if (!string.IsNullOrEmpty(toolTip))
                    toolTip += Environment.NewLine;
                toolTip += $"{versionHeader}{_commandType.Assembly.GetName().Version}";
            }

            SetTooltipInternal(toolTip);

            return this;
        }

        /// <inheritdoc />
        public IButton SetLargeImage(Uri imageUri)
        {
            if (imageUri != null)
            {
                SetLargeImageInternal(new BitmapImage(imageUri));
            }

            return this;
        }

        /// <inheritdoc />
        public IButton SetSmallImage(Uri imageUri)
        {
            if (imageUri != null)
            {
                SetSmallImageInternal(new BitmapImage(imageUri));
            }

            return this;
        }

        /// <inheritdoc />
        public IButton SetDescription(string description)
        {
            SetDescriptionInternal(description);
            return this;
        }

        /// <inheritdoc />
        public IButton SetHelpUrl(string url)
        {
            SetHelpUrlInternal(url);
            return this;
        }

        /// <summary>
        /// Sets tooltip
        /// </summary>
        /// <param name="tooltip">Tooltip text</param>
        protected abstract void SetTooltipInternal(string tooltip);

        /// <summary>
        /// Sets large image
        /// </summary>
        /// <param name="image">Image</param>
        protected abstract void SetLargeImageInternal(BitmapImage image);

        /// <summary>
        /// Sets small image
        /// </summary>
        /// <param name="image">Image</param>
        protected abstract void SetSmallImageInternal(BitmapImage image);

        /// <summary>
        /// Sets description
        /// </summary>
        /// <param name="description">Description text</param>
        protected abstract void SetDescriptionInternal(string description);

        /// <summary>
        /// Sets URL for help
        /// </summary>
        /// <param name="url">Url</param>
        protected abstract void SetHelpUrlInternal(string url);
    }
}