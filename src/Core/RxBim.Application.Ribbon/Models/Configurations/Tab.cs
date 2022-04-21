namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;

    /// <summary>
    /// Reprsents a ribbon tab configuration.
    /// </summary>
    public class Tab
    {
        /// <summary>
        /// The tab name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The tab panel list.
        /// </summary>
        public List<Panel> Panels { get; set; } = new();
    }
}