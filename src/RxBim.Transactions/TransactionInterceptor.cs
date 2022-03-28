namespace RxBim.Transactions
{
    using System.Reflection;
    using Abstractions;
    using Attributes;
    using Castle.DynamicProxy;

    /// <inheritdoc />
    public class TransactionInterceptor : IInterceptor
    {
        private readonly ITransactionFactory _factory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="factory">transaction factory</param>
        public TransactionInterceptor(ITransactionFactory factory)
        {
            _factory = factory;
        }

         /// <inheritdoc />
        public void Intercept(IInvocation invocation)
        {
            var customAttribute = invocation.MethodInvocationTarget.GetCustomAttribute<TransactionalAttribute>();
            if (customAttribute != null)
            {
                var transactionName = customAttribute.TransactionName ?? invocation.Method.Name;
                using var t = _factory.Create(transactionName);

                try
                {
                    t.Start(transactionName);
                    invocation.Proceed();
                    t.Commit();
                }
                catch
                {
                    t.Rollback();
                    throw;
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}