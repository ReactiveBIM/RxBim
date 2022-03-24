namespace RxBim.Transactions.Abstractions
{
    /// <summary>
    /// Transaction factory
    /// </summary>
    public interface ITransactionFactory
    {
        /// <summary>
        /// Create the new instance of <see cref="ITransaction"/>
        /// </summary>
        /// <param name="transactionName">transaction name</param>
        ITransaction Create(string transactionName = null);
    }
}