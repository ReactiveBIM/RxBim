namespace RxBim.Transactions.Tests.TestClasses
{
    using System;
    using Attributes;

    public class Ab2 : IA, IB
    {
        [Transactional]
        public void MethodA()
        {
            throw new NotImplementedException();
        }

        [Transactional]
        public void MethodB()
        {
            throw new NotImplementedException();
        }

        public void BadMethod()
        {
        }
    }
}