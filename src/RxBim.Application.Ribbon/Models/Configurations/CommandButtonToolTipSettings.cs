namespace RxBim.Application.Ribbon.Models.Configurations
{
    /// <summary>
    /// Command button tooltip settings
    /// </summary>
    public class CommandButtonToolTipSettings
    {
        /// <summary>
        /// If true, a version number will be added to the tooltip text
        /// </summary>
        public bool AddVersion { get; set; } = true;

        /// <summary>
        /// Version info header (prefix)
        /// Examples: "v" -> "v1.0.0", "Ver." -> "Ver.1.0.0", "Version: " -> "Version: 1.0.0"
        /// </summary>
        public string VersionHeader { get; set; } = string.Empty;
    }
}