namespace RxBim.Transactions.Tests.TestClasses
{
    using System;
    using Attributes;

    public class Ab : IA, IB
    {
        [Transactional]
        public void MethodA()
        {
            throw new Exception();
        }

        [Transactional]
        public void MethodB()
        {
            throw new Exception();
        }
    }
}