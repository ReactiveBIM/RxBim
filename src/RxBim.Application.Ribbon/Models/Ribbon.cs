namespace RxBim.Application.Ribbon.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Plugin ribbon menu configuration
    /// </summary>
    public class Ribbon
    {
        /// <summary>
        /// Ribbon tabs
        /// </summary>
        public List<Tab> Tabs { get; set; } = new ();
    }
}