namespace RxBim.Transactions.Abstractions
{
    /// <summary>
    /// Indicates that a type can provide transaction proxies 
    /// </summary>
    public interface ITransactionProxyProvider
    {
        /// <summary>
        /// Setups container to provide proxies
        /// </summary>
        void SetupContainer();
    }
}