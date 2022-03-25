namespace RxBim.Transactions.Tests.TestClasses
{
    using Attributes;
    using Setup;

    public class Virtual
    {
        [Transactional]
        public virtual void VirtualMethod()
        {
            TestClass.Result += nameof(VirtualMethod) + "\n";
        }
    }
}