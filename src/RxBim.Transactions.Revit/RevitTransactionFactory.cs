namespace RxBim.Transactions.Revit
{
    using Abstractions;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Defines a transaction factory for Revit.
    /// </summary>
    public class RevitTransactionFactory : ITransactionFactory
    {
        private readonly Document _document;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="document">A Revit document.</param>
        public RevitTransactionFactory(Document document)
        {
            _document = document;
        }

        /// <inheritdoc />
        public ITransaction Create(string transactionName = null)
        {
            return new RevitTransaction(new Transaction(_document, transactionName));
        }
    }
}