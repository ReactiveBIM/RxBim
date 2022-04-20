namespace RxBim.Transactions.Autocad
{
    using Abstractions;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Defines a transaction factory for Autocad.
    /// </summary>
    public class AutocadTransactionFactory : ITransactionFactory
    {
        private readonly TransactionManager _transactionManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="database">Autocad database</param>
        public AutocadTransactionFactory(Database database)
        {
            _transactionManager = database.TransactionManager;
        }

        /// <inheritdoc />
        public ITransaction Create(string transactionName = null)
        {
            return new AutocadTransaction(_transactionManager.StartTransaction());
        }
    }
}