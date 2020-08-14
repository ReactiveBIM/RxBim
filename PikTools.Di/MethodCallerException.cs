namespace PikTools.Di
{
    using System;

    /// <summary>
    /// Исключение вызова метода команды или приложения Revit
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