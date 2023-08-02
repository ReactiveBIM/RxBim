namespace RxBim.Di.Tests
{
    extern alias msdi;
    using System;
    using FluentAssertions;
    using Shared;
    using TestObjects;
    using Xunit;
    using msdi::Microsoft.Extensions.DependencyInjection;

    public class MethodCallerTests
    {
        [Fact]
        public void CallerShouldThrowExceptionOnBadMethodName()
        {
            var badObject = new BadMethodNameObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            var services = new ServiceCollection();
            Action act = () => methodCaller.InvokeMethod(services, "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Fact]
        public void CallerShouldThrowExceptionOnBadReturnType()
        {
            var badObject = new BadReturnTypeObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            
            var services = new ServiceCollection();
            Action act = () => methodCaller.InvokeMethod(services, "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Fact]
        public void CallerShouldReturnCorrectData()
        {
            var testObject = new TestObject();

            var methodCaller = new MethodCaller<int>(testObject);
            var result = 0;
            var services = new ServiceCollection();
            Action act = () => result = methodCaller.InvokeMethod(services, "Execute");

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