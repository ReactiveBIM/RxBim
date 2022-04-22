namespace RxBim.Di
{
    using System;

    /// <summary>
    /// The exception of execution external application or command.
    /// </summary>
    public class MethodCallerException : Exception
    {
        /// <inheritdoc />
        public MethodCallerException()
        {
        }

        /// <inheritdoc />
        public MethodCallerException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public MethodCallerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}