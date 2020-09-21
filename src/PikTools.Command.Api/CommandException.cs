namespace PikTools.Command.Api
{
    using System;

    /// <summary>
    /// Исключение выполнения комманды Revit
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