namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Core.Internal;
    using Castle.DynamicProxy.Internal;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;
    using Transactions.Abstractions;
    using Transactions.Attributes;

    /// <summary>
    /// The implementation of <see cref="IContainer"/> based on <see cref="SimpleInjector"/>
    /// </summary>
    public class SimpleInjectorContainer : IContainer, ITransactionProxyProvider
    {
        private readonly Container _container;

        /// <summary>
        /// ctor
        /// </summary>
        public SimpleInjectorContainer()
        {
            _container = new Container();
            _container.Options.EnableAutoVerification = false;
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        /// <inheritdoc />
        public IContainer Add(Type serviceType, Type implementationType, Lifetime lifetime)
        {
            _container.Register(serviceType, implementationType, GetLifestyle(lifetime));
            return this;
        }

        /// <inheritdoc />
        public IContainer Add(Type serviceType, Func<object> implementationFactory, Lifetime lifetime)
        {
            _container.Register(serviceType, implementationFactory, GetLifestyle(lifetime));
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton(Type serviceType, object implementationInstance)
        {
            _container.Register(serviceType, () => implementationInstance, Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddInstance(Type serviceType, object implementationInstance)
        {
            _container.RegisterInstance(serviceType, implementationInstance);
            return this;
        }

        /// <inheritdoc />
        public IContainer Decorate(Type serviceType, Type decoratorType)
        {
            _container.RegisterDecorator(serviceType, decoratorType);
            return this;
        }

        /// <inheritdoc />
        public IEnumerable<Registration> GetCurrentRegistrations()
        {
            return _container.GetCurrentRegistrations().Select(x => new Registration(x.ServiceType));
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        /// <inheritdoc />
        public IContainerScope CreateScope()
        {
            var scope = AsyncScopedLifestyle.BeginScope(_container);
            return new SimpleInjectorScope(scope, this);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _container.Dispose();
        }

        private Lifestyle GetLifestyle(Lifetime lifetime) => lifetime switch
        {
            Lifetime.Transient => Lifestyle.Transient,
            Lifetime.Scoped => Lifestyle.Scoped,
            Lifetime.Singleton => Lifestyle.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };

        /// <inheritdoc />
        public void SetupProxy()
        {
            _container.ExpressionBuilding += ContainerOnExpressionBuilding;
            _container.ExpressionBuilt += ContainerOnExpressionBuilt;
        }

        private void ContainerOnExpressionBuilding(object sender, ExpressionBuildingEventArgs e)
        {
            var type = e.KnownImplementationType;
           
        }

        private void ContainerOnExpressionBuilt(object sender, ExpressionBuiltEventArgs e)
        {
            //     var type = typeof(ProxyGenerator);
//     var proxyGenerator = Expression.New(type);
//
//     var methodInfos = type
//         .GetMethods();
//     var methodInfo = methodInfos
//         .FirstOrDefault(x => x.Name == "CreateClassProxyWithTarget" &&
//                              x.GetParameters().Length == 3 &&
//                              x.GetParameters().Skip(1).First().ParameterType == typeof(object));
//
//     var unaryExpression = Expression.Convert(Expression.New(typeof(Interceptor)), typeof(IInterceptor));
//     var newArrayExpression = Expression.NewArrayInit(typeof(IInterceptor),
//         unaryExpression);
//     var constantExpression = Expression.Constant(eventArgs.KnownImplementationType);
//     var methodCallExpression =
//         Expression.Call(proxyGenerator, methodInfo, constantExpression, eventArgs.Expression, newArrayExpression);
//
//     eventArgs.Expression = Expression.Convert(methodCallExpression, eventArgs.KnownImplementationType);
        }
    }
}