namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a plugin ribbon menu configuration.
    /// </summary>
    public class Ribbon
    {
        /// <summary>
        /// Ribbon tabs.
        /// </summary>
        public List<Tab> Tabs { get; set; } = new();

        /// <summary>
        /// If true, the version number will be added to the tooltip text for a command item.
        /// </summary>
        public bool DisplayVersion { get; set; } = true;

        /// <summary>
        /// Version information prefix for a command item tooltip.
        /// </summary>
        /// <remarks>
        /// Examples: "v" -> "v1.0.0", "Ver." -> "Ver.1.0.0", "Version: " -> "Version: 1.0.0".
        /// Used only when the <see cref="DisplayVersion"/> is set to true.
        /// </remarks>
        public string VersionPrefix { get; set; } = string.Empty;
    }
}