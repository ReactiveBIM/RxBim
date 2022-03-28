namespace RxBim.Transactions.Tests
{
    using System;
    using Di.Exceptions;
    using Extensions;
    using FluentAssertions;
    using TestClasses;
    using Xunit;

    public class TypeCheckTests
    {
        [Fact]
        public void NonVirtualMethodsShouldThrow()
        {
            Action act = () => typeof(NonVirtual).CheckType();
            act.Should().Throw<RegistrationException>().WithMessage(
                "Method `NonVirtualMethod` of type RxBim.Transactions.Tests.TestClasses.NonVirtual can't be transactional. " +
                "Transactional methods should be virtual or" +
                " implemented from any interface.");
        }

        [Fact]
        public void VirtualMethodsShouldNoThrow()
        {
            Action act = () => typeof(Virtual).CheckType();
            act.Should().NotThrow();
        }

        [Fact]
        public void NonVirtualMethodsShouldThrow2()
        {
            Action act = () => typeof(A).CheckType();
            act.Should().Throw<RegistrationException>().WithMessage(
                "Method `BadMethod` of type RxBim.Transactions.Tests.TestClasses.A can't be transactional. " +
                "Transactional methods should be virtual or" +
                " implemented from any interface.");
        }

        [Fact]
        public void NonVirtualMethodsShouldThrow3()
        {
            Action act = () => typeof(A2).CheckType();
            act.Should().Throw<RegistrationException>().WithMessage(
                "Method `BadMethod` of type RxBim.Transactions.Tests.TestClasses.A2 can't be transactional. " +
                "Transactional methods should be virtual or" +
                " implemented from any interface.");
        }

        [Fact]
        public void InterfaceMethodsShouldNotThrow()
        {
            Action act = () => typeof(Ab).CheckType();
            act.Should().NotThrow();
        }

        [Fact]
        public void InterfaceAndNonTransactionalMethodsShouldNotThrow()
        {
            Action act = () => typeof(Ab2).CheckType();
            act.Should().NotThrow();
        }

        [Fact]
        public void NonVirtualMethodsShouldThrow4()
        {
            Action act = () => typeof(Ab3).CheckType();
            act.Should().Throw<RegistrationException>().WithMessage(
                "Method `BadMethod` of type RxBim.Transactions.Tests.TestClasses.Ab3 can't be transactional. " +
                "Transactional methods should be virtual or" +
                " implemented from any interface.");
        }
    }
}