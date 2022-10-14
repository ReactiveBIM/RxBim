namespace RxBim.Shared
{
    /// <summary>
    /// Specifies the result of an operation.
    /// </summary>
    public enum Result
    {
        /// <summary>
        /// An operation succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// An operation cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// An operation failed.
        /// </summary>
        Failed
    }
}