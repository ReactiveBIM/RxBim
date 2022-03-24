namespace RxBim.Di.Exceptions
{
    using System;

    /// <summary>
    /// Represents errors during dependency registration
    /// </summary>
    public class RegistrationException : Exception
    {
        /// <inheritdoc />
        public RegistrationException()
        {
        }

        /// <inheritdoc />
        public RegistrationException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        public RegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}