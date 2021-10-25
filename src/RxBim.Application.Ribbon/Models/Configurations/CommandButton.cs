namespace RxBim.Application.Ribbon.Models.Configurations
{
    /// <summary>
    /// Button to run a command
    /// </summary>
    public class CommandButton : Button
    {
        /// <summary>
        /// Command class type <see cref="System.Type.FullName"/>
        /// </summary>
        public string? CommandType { get; set; }

        /// <summary>
        /// Tooltip settings
        /// </summary>
        public CommandButtonToolTipSettings ToolTipSettings { get; set; } = new ();
    }
}