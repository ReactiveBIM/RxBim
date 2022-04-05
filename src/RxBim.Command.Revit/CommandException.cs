namespace RxBim.Command.Revit
{
    using System;

    /// <summary>
    /// Revit command exception.
    /// </summary>
    public class CommandException : Exception
    {
        /// <inheritdoc />
        public CommandException()
        {
        }

        /// <inheritdoc />
        public CommandException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public CommandException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}