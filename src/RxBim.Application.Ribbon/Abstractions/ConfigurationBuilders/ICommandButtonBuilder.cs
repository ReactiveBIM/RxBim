namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
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
        /// <param name="versionHeader">
        /// Version info header (prefix)
        /// Examples: "v" -> "v1.0.0", "Ver." -> "Ver.1.0.0", "Version: " -> "Version: 1.0.0"
        /// </param>
        ICommandButtonBuilder SetToolTip(string toolTip, bool addVersion = true, string versionHeader = "");
    }
}