namespace RxBim.Application.Ribbon.Abstractions
{
    using System;

    /// <summary>
    /// Ribbon button
    /// </summary>
    public interface IButtonBuilder
    {
        /// <summary>
        /// Sets a tooltip for the button
        /// </summary>
        /// <param name="toolTip">Tooltip text</param>
        /// <param name="addVersion">If true, a version number will be added to the tooltip text</param>
        /// <param name="versionHeader">Prefix for version info</param>
        IButtonBuilder SetToolTip(string toolTip, bool addVersion = true, string versionHeader = "");

        /// <summary>
        /// Sets a large image for the button
        /// </summary>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        IButtonBuilder SetLargeImage(Uri imageUri);

        /// <summary>
        /// Sets a small image for the button
        /// </summary>
        /// <param name="imageUri">Image <see cref="Uri"/></param>
        IButtonBuilder SetSmallImage(Uri imageUri);

        /// <summary>
        /// Set description for the button
        /// </summary>
        /// <param name="description">Description text</param>
        IButtonBuilder SetDescription(string description);

        /// <summary>
        /// Sets the help URL for the button command
        /// </summary>
        /// <param name="url">URL address</param>
        IButtonBuilder SetHelpUrl(string url);
    }
}