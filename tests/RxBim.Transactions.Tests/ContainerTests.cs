namespace RxBim.Transactions.Tests
{
    using System;
    using Abstractions;
    using Di;
    using Di.Exceptions;
    using Extensions;
    using FluentAssertions;
    using Setup;
    using TestClasses;
    using Xunit;

    public class ContainerTests
    {
        public ContainerTests()
        {
            TestClass.Result = string.Empty;
        }

        [Fact]
        public void ServiceProvideShouldThrowAtSetup()
        {
            var serviceProviderContainer = new ServiceProviderContainer();
            Action act = () => serviceProviderContainer.TrySetupProxy();

            act.Should().Throw<RegistrationException>();
        }

        [Fact]
        public void SimpleInjectorShouldNotThrowAtSetup()
        {
            var simpleInjectorContainer = new SimpleInjectorContainer();
            Action act = () => simpleInjectorContainer.TrySetupProxy();

            act.Should().NotThrow();
        }

        [Fact]
        public void ContainerShouldReturnProxyFromClass()
        {
            var container = new SimpleInjectorContainer();
            container.TrySetupProxy()
                .AddTransient<ITransactionFactory, TestTransactionFactory>()
                .AddTransient<Virtual>();

            var service = container.GetService<Virtual>();

            service.GetType().Should().NotBe(typeof(Virtual));
        }

        [Fact]
        public void ContainerShouldReturnProxyFromInterface()
        {
            var container = new SimpleInjectorContainer();
            container.TrySetupProxy()
                .AddTransient<ITransactionFactory, TestTransactionFactory>()
                .AddTransient<IA, Ab2>()
                .AddTransient<IB, Ab2>();

            var service = container.GetService<IA>();
            var service2 = container.GetService<IB>();

            service.GetType().Should().NotBe(typeof(Ab2));
            service2.GetType().Should().NotBe(typeof(Ab2));
        }

        [Fact]
        public void TransactionShouldBeCommited()
        {
            var container = new SimpleInjectorContainer();
            container.TrySetupProxy()
                .AddTransient<ITransactionFactory, TestTransactionFactory>()
                .AddTransient<Virtual>();

            var service = container.GetService<Virtual>();
            service.VirtualMethod();

            TestClass.Result.Should()
                .Be("Transaction started\n" +
                    $"{nameof(Virtual.VirtualMethod)}\n" +
                    "Transaction commited\n" +
                    "Transaction disposed");
        }

        [Fact]
        public void TransactionShouldBeRollback()
        {
            var container = new SimpleInjectorContainer();
            container.TrySetupProxy()
                .AddTransient<ITransactionFactory, TestTransactionFactory>()
                .AddTransient<IA, Ab>();

            var service = container.GetService<IA>();
            Action act = () => service.MethodA();

            act.Should().Throw<Exception>();

            TestClass.Result.Should()
                .Be("Transaction started\n" +
                    "Transaction rollback\n" +
                    "Transaction disposed");
        }
    }
}