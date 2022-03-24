namespace RxBim.Transactions.Abstractions
{
    using System;

    /// <summary>
    /// Transaction interface
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Starts transaction
        /// </summary>
        /// <param name="transactionName">transaction name</param>
        void Start(string transactionName = null);

        /// <summary>
        /// Commits transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks transaction
        /// </summary>
        void Rollback();
    }
}