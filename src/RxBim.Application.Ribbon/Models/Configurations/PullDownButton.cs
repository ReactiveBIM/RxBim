namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Pull-down button.
    /// </summary>
    public class PullDownButton : Button
    {
        /// <summary>
        /// Buttons contained in the pull-down button.
        /// </summary>
        public List<CommandButton> CommandButtonsList { get; set; } = new();
    }
}