namespace RxBim.Transactions.Tests.Setup
{
    using Abstractions;

    public class TestTransactionFactory : ITransactionFactory
    {
        public ITransaction Create(string? transactionName = null)
        {
            return new TestTransaction();
        }
    }
}