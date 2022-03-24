namespace RxBim.Transactions.Revit
{
    using Abstractions;
    using Autodesk.Revit.DB;

    /// <inheritdoc />
    public class RevitTransactionFactory : ITransactionFactory
    {
        private readonly Document _document;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="document">Revit document</param>
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