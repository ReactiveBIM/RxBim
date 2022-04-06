namespace RxBim.Transactions.Abstractions
{
    using System;

    /// <summary>
    /// Defines an interface for transactions.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <param name="transactionName">The transaction name.</param>
        void Start(string transactionName = null);

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        void Rollback();
    }
}