namespace RxBim.Transactions.Revit.Extensions
{
    using Abstractions;
    using Di;
    using Transactions.Extensions;

    /// <summary>
    /// Container extensions
    /// </summary>
    public static class ContainerExtension
    {
        /// <summary>
        /// Adds transaction factory in DI container
        /// </summary>
        /// <param name="container">DI container</param>
        public static IContainer AddTransactions(IContainer container)
        {
            return container
                .TrySetupProxy()
                .AddTransient<ITransactionFactory, RevitTransactionFactory>();
        }
    }
}