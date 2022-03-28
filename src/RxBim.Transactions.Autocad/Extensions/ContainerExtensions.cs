namespace RxBim.Transactions.Autocad.Extensions
{
    using Abstractions;
    using Di;
    using Transactions.Extensions;

    /// <summary>
    /// Container extensions
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds transaction factory in DI container
        /// </summary>
        /// <param name="container">DI container</param>
        public static IContainer AddTransactions(this IContainer container)
        {
            return container
                .TrySetupProxy()
                .AddTransient<ITransactionFactory, AutocadTransactionFactory>();
        }
    }
}