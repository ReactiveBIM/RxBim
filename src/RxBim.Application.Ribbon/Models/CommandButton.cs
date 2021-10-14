namespace RxBim.Application.Ribbon.Models
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
        /// Help url for command
        /// </summary>
        public string? HelpUrl { get; set; }
    }
}