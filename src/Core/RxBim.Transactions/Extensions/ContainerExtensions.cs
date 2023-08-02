namespace RxBim.Transactions.Extensions
{
    using Abstractions;
    using Castle.DynamicProxy;
    using Di.Exceptions;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds transaction proxies to a DI container.
        /// </summary>
        /// <param name="services">The DI container.</param>
        public static IServiceCollection SetupProxy(this IServiceCollection services)
        {
            if (services is ITransactionProxyProvider proxyProvider)
            {
                proxyProvider.SetupContainerForProxyGeneration();
            }
            else
            {
                throw new RegistrationException(
                    "Current container configuration doesn't implement transaction proxy functionality!");
            }

            return services.AddTransactionInterceptor();
        }

        /// <summary>
        /// Adds transaction interceptors into a DI container.
        /// </summary>
        /// <param name="services">The DI container.</param>
        private static IServiceCollection AddTransactionInterceptor(this IServiceCollection services)
        {
            return services.AddTransient<IInterceptor, TransactionInterceptor>();
        }
    }
}