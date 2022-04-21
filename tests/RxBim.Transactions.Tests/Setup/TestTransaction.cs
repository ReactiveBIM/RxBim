namespace RxBim.Transactions.Tests.Setup
{
    using Abstractions;

    public class TestTransaction : ITransaction
    {
        public void Dispose()
        {
            TestClass.Result += "Transaction disposed";
        }

        public void Start(string? transactionName = null)
        {
            TestClass.Result += "Transaction started\n";
        }

        public void Commit()
        {
            TestClass.Result += "Transaction commited\n";
        }

        public void Rollback()
        {
            TestClass.Result += "Transaction rollback\n";
        }
    }
}