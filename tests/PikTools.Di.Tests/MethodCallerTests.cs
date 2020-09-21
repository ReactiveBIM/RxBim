namespace PikTools.Di.Tests
{
    using System;
    using FluentAssertions;
    using Shared;
    using SimpleInjector;
    using TestObjects;
    using Xunit;

    public class MethodCallerTests
    {
        [Fact]
        public void CallerShouldThrowExceptionOnBadMethodName()
        {
            var badObject = new BadMethodNameObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            Action act = () => methodCaller.InvokeCommand(new Container(), "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Fact]
        public void CallerShouldThrowExceptionOnBadReturnType()
        {
            var badObject = new BadReturnTypeObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            Action act = () => methodCaller.InvokeCommand(new Container(), "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Fact]
        public void CallerShouldReturnCorrectData()
        {
            var testObject = new TestObject();

            var methodCaller = new MethodCaller<int>(testObject);
            int result = 0;
            Action act = () => result = methodCaller.InvokeCommand(new Container(), "Execute");

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