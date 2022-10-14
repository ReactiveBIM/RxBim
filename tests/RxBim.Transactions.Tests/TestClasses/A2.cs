namespace RxBim.Transactions.Tests.TestClasses
{
    using System;
    using Attributes;

    public class A2 : IA
    {
        [Transactional]
        public void MethodA()
        {
            throw new NotImplementedException();
        }

        [Transactional]
        public void BadMethod()
        {
        }
    }
}