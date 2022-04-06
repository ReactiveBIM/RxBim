namespace RxBim.Transactions.Extensions
{
    using Abstractions;
    using Castle.DynamicProxy;
    using Di;
    using Di.Exceptions;

    /// <summary>
    /// Extensions for <see cref="IContainer"/>
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds transaction proxies to a DI container.
        /// </summary>
        /// <param name="container">The DI container.</param>
        public static IContainer SetupProxy(this IContainer container)
        {
            if (container is ITransactionProxyProvider proxyProvider)
            {
                proxyProvider.SetupContainerForProxyGeneration();
            }
            else
            {
                throw new RegistrationException(
                    "Current container configuration doesn't implement transaction proxy functionality!");
            }

            return container.AddTransactionInterceptor();
        }

        /// <summary>
        /// Adds transaction interceptors into a DI container.
        /// </summary>
        /// <param name="container">The DI container.</param>
        private static IContainer AddTransactionInterceptor(this IContainer container)
        {
            return container.AddTransient<IInterceptor, TransactionInterceptor>();
        }
    }
}