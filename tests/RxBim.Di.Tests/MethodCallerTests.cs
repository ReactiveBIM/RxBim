namespace RxBim.Di.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Shared;
    using TestObjects;
    using Xunit;

    public class MethodCallerTests
    {
        public static IEnumerable<object[]> GetContainers()
        {
            yield return new object[] { new SimpleInjectorContainer() };
            yield return new object[] { new ServiceProviderContainer() };
        }

        [Theory]
        [MemberData(nameof(GetContainers))]
        public void CallerShouldThrowExceptionOnBadMethodName(IContainer container)
        {
            var badObject = new BadMethodNameObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            Action act = () => methodCaller.InvokeMethod(container, "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Theory]
        [MemberData(nameof(GetContainers))]
        public void CallerShouldThrowExceptionOnBadReturnType(IContainer container)
        {
            var badObject = new BadReturnTypeObject();

            var methodCaller = new MethodCaller<PluginResult>(badObject);
            Action act = () => methodCaller.InvokeMethod(container, "Execute");

            act.Should().Throw<MethodCallerException>();
        }

        [Theory]
        [MemberData(nameof(GetContainers))]
        public void CallerShouldReturnCorrectData(IContainer container)
        {
            var testObject = new TestObject();

            var methodCaller = new MethodCaller<int>(testObject);
            int result = 0;
            Action act = () => result = methodCaller.InvokeMethod(container, "Execute");

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