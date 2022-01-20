namespace RxBim.Di.Tests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Logs;
    using TestDependencies;
    using Xunit;

    public class ContainerTests
    {
        public static IEnumerable<object[]> GetContainers()
        {
            yield return new object[] { new SimpleInjectorContainer() };
            yield return new object[] { new ServiceProviderContainer() };
        }

        [Theory]
        [MemberData(nameof(GetContainers))]
        public void SameServiceSameScope(IContainer container)
        {
            container.Add(typeof(ServiceA), typeof(ServiceA), Lifetime.Scoped);
            ServiceBase svc1;
            ServiceBase svc2;
            using (var scope = container.CreateScope())
            {
                var scopedContainer = scope.GetContainer();
                svc1 = scopedContainer.GetService<ServiceA>();
                svc2 = scopedContainer.GetService<ServiceA>();

                svc1.Should().BeSameAs(svc2);
                svc1.Disposed.Should().BeFalse();
            }

            svc1.Disposed.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetContainers))]
        public void SameServiceOtherScope(IContainer container)
        {
            container.Add(typeof(ServiceA), typeof(ServiceA), Lifetime.Scoped);
            ServiceBase svc1;
            ServiceBase svc2;
            using (var scope = container.CreateScope())
            {
                var scopedContainer = scope.GetContainer();
                svc1 = scopedContainer.GetService<ServiceA>();
                svc1.Disposed.Should().BeFalse();
            }

            using (var scope = container.CreateScope())
            {
                var scopedContainer = scope.GetContainer();
                svc2 = scopedContainer.GetService<ServiceA>();

                svc1.Should().NotBeSameAs(svc2);
                svc2.Disposed.Should().BeFalse();
            }

            svc1.Disposed.Should().BeTrue();
            svc2.Disposed.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetContainers))]
        public void SameSingletonService(IContainer container)
        {
            container.Add(typeof(ServiceA), typeof(ServiceA), Lifetime.Singleton);

            ServiceBase svc1 = container.GetService<ServiceA>();
            ServiceBase svc2 = container.GetService<ServiceA>();

            svc1.Should().BeSameAs(svc2);
            svc1.Disposed.Should().BeFalse();

            container.Dispose();
            svc1.Disposed.Should().BeTrue();
        }

        [Fact]
        public void LoggerInSingletonService()
        {
            var container = new SimpleInjectorContainer();
            container.AddLogs();
            container.AddSingleton<ServiceWithLogger>();

            Action act = () => container.GetService<ServiceWithLogger>();

            act.Should().NotThrow();
        }
    }
}