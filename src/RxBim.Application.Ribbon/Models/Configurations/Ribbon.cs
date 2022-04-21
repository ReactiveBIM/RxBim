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
        /// If true, the version number will be added to the tooltip text for a command element.
        /// </summary>
        public bool AddVersionToCommandTooltip { get; set; } = true;

        /// <summary>
        /// Version information prefix for a command element tooltip.
        /// </summary>
        /// <remarks>
        /// Examples: "v" -> "v1.0.0", "Ver." -> "Ver.1.0.0", "Version: " -> "Version: 1.0.0".
        /// Used only when the <see cref="AddVersionToCommandTooltip"/> is set to true.
        /// </remarks>
        public string CommandTooltipVersionHeader { get; set; } = string.Empty; // todo rename to prefix
    }
}