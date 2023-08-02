namespace RxBim.Transactions.Revit.Extensions
{
    using Abstractions;
    using Microsoft.Extensions.DependencyInjection;
    using Transactions.Extensions;

    /// <summary>
    /// Container extensions
    /// </summary>
    public static class ContainerExtension
    {
        /// <summary>
        /// Adds transaction factory in DI container
        /// </summary>
        /// <param name="services">DI container</param>
        public static IServiceCollection AddTransactions(this IServiceCollection services)
        {
            return services
                .SetupProxy()
                .AddTransient<ITransactionFactory, RevitTransactionFactory>();
        }
    }
}