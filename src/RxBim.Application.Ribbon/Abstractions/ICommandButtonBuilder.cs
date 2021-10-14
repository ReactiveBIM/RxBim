namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Command button builder
    /// </summary>
    public interface ICommandButtonBuilder : IButtonBuilder
    {
        /// <summary>
        /// Sets a tooltip for the button
        /// </summary>
        /// <param name="toolTip">Tooltip text</param>
        /// <param name="addVersion">If true, a version number will be added to the tooltip text</param>
        /// <param name="versionHeader">Prefix for version info</param>
        ICommandButtonBuilder SetToolTip(string toolTip, bool addVersion = true, string versionHeader = "");

        /// <summary>
        /// Sets the help URL for the button command
        /// </summary>
        /// <param name="url">URL address</param>
        ICommandButtonBuilder SetHelpUrl(string url);
    }
}