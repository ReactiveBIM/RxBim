namespace RxBim.Transactions.Tests.TestClasses
{
    using System;
    using Attributes;

    public class Ab3 : IA, IB
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

        [Transactional]
        public void BadMethod()
        {
        }
    }
}