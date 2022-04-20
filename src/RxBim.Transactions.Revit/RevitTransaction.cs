namespace RxBim.Transactions.Revit
{
    using Abstractions;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Represents a transaction in Revit.
    /// </summary>
    internal class RevitTransaction : ITransaction
    {
        private readonly Transaction _revitTransaction;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="revitTransaction">Revit transaction</param>
        public RevitTransaction(Transaction revitTransaction)
        {
            _revitTransaction = revitTransaction;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _revitTransaction.Dispose();
        }

        /// <inheritdoc />
        public void Start(string transactionName = null)
        {
            _revitTransaction.Start(transactionName);
        }

        /// <inheritdoc />
        public void Commit()
        {
            _revitTransaction.Commit();
        }

        /// <inheritdoc />
        public void Rollback()
        {
            _revitTransaction.RollBack();
        }
    }
}