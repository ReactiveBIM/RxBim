namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a pull-down button.
    /// </summary>
    public class PullDownButton : Button
    {
        /// <summary>
        /// The buttons contained in the pull-down button.
        /// </summary>
        public List<CommandButton> CommandButtonsList { get; set; } = new();
    }
}