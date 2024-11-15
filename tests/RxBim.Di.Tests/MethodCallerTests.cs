namespace RxBim.Di.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using TestObjects;
    using Xunit;

    public class MethodCallerTests
    {
        [Fact]
        public void CallerShouldThrowExceptionOnBadMethodName()
        {
            var badObject = new BadMethodNameObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            Action act = () => methodCaller.InvokeMethod(serviceProvider, "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Fact]
        public void CallerShouldThrowExceptionOnBadReturnType()
        {
            var badObject = new BadReturnTypeObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            Action act = () => methodCaller.InvokeMethod(serviceProvider, "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Fact]
        public void CallerShouldReturnCorrectData()
        {
            var testObject = new TestObject();

            var methodCaller = new MethodCaller<int>(testObject);
            var result = 0;
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            Action act = () => result = methodCaller.InvokeMethod(serviceProvider, "Execute");

            act.Should().NotThrow();
            result.Should().Be(100);
        }

        [Fact]
        public void CallerSourceObjectShouldBeCorrect()
        {
            var testObject = new TestObject();

            var methodCaller = new MethodCaller<int>(testObject);
            methodCaller.SourceObjectType.FullName.Should().Be(typeof(TestObject).FullName);
        }
    }
}