namespace RxBim.Application.Ribbon.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Pull-down button
    /// </summary>
    public class PullDownButton : Button
    {
        /// <summary>
        /// Buttons contained in the pull-down button
        /// </summary>
        public List<CommandButton>? Buttons { get; set; }
    }
}