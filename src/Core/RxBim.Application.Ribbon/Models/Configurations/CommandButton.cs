namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Represents a button that invokes a command.
    /// </summary>
    public class CommandButton : Button
    {
        /// <summary>
        /// A command to be invoked type name.
        /// </summary>
        public string? CommandType { get; set; }
    }
}