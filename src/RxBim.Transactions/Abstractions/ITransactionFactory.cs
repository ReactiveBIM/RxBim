namespace RxBim.Transactions.Abstractions
{
    /// <summary>
    /// Defines an interface for transaction factories.
    /// </summary>
    public interface ITransactionFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="ITransaction"/>.
        /// </summary>
        /// <param name="transactionName">The transaction name.</param>
        ITransaction Create(string? transactionName = null);
    }
}