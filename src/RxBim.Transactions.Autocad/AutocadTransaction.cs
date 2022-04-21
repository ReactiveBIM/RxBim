namespace RxBim.Transactions.Autocad
{
    using Abstractions;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Represents a transaction in Autocad.
    /// </summary>
    public class AutocadTransaction : ITransaction
    {
        private readonly Transaction _transaction;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="transaction">An Autocad transaction</param>
        public AutocadTransaction(Transaction transaction)
        {
            _transaction = transaction;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _transaction.Dispose();
        }

        /// <inheritdoc />
        public void Start(string? transactionName = null)
        {
            // transaction is already started after creation
        }

        /// <inheritdoc />
        public void Commit()
        {
            _transaction.Commit();
        }

        /// <inheritdoc />
        public void Rollback()
        {
            _transaction.Abort();
        }
    }
}