namespace RxBim.Application.Ribbon.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Ribbon panel configuration
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// Panel name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Buttons on the panel
        /// </summary>
        public List<Button> Buttons { get; set; }
    }
}