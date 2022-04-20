namespace RxBim.Transactions.Tests.TestClasses
{
    using Attributes;

    public class NonVirtual
    {
        [Transactional]
        public void NonVirtualMethod()
        {
        }
    }
}