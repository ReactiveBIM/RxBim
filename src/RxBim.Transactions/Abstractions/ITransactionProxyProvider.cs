namespace RxBim.Transactions.Abstractions
{
    /// <summary>
    /// Defines an interface for containers providing transaction proxies.
    /// </summary>
    public interface ITransactionProxyProvider
    {
        /// <summary>
        /// Sets up the container to provide proxies.
        /// </summary>
        void SetupContainerForProxyGeneration();
    }
}