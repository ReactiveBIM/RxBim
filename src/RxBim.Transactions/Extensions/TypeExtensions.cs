namespace RxBim.Transactions.Extensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Attributes;
    using Castle.Core.Internal;
    using Castle.DynamicProxy;
    using Castle.DynamicProxy.Internal;
    using Di.Exceptions;

    /// <summary>
    /// Type extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if any type methods has <see cref="TransactionalAttribute"/> 
        /// </summary>
        /// <param name="type">type</param>
        public static bool IsTransactional(this Type type)
        {
            return ImplementationsOfRequestedTypeHasTransactionalAttribute(type) ||
                   RequestedTypeHasTransactionalAttribute(type);
        }

        /// <summary>
        /// Checks that type can be wrapped by transaction proxy
        /// </summary>
        /// <param name="type">type</param>
        public static void CheckIfTypeCanBeTransactional(this Type type)
        {
            var methods = type.GetMethods().Where(x => x.GetAttribute<TransactionalAttribute>() != null).ToList();
            if (methods.Any())
            {
                var allInterfaces = type.GetAllInterfaces();
                var interfaceMethods = allInterfaces.SelectMany(x => type.GetInterfaceMap(x).TargetMethods).ToList();

                foreach (var method in methods)
                {
                    if (!interfaceMethods.Contains(method) && !method.IsVirtual)
                    {
                        throw new RegistrationException(
                            $"Method `{method.Name}` of type {type.FullName} can't be transactional. " +
                            "Transactional methods should be virtual or implemented from any interface.");
                    }
                }
            }
        }

        /// <summary>
        /// Modifies expression. Wraps base expression into proxy generation
        /// </summary>
        /// <param name="implementationType">implementation type</param>
        /// <param name="baseExpression">base expression</param>
        /// <param name="interceptors">expression for creating interceptors</param>
        public static Expression ModifyInstanceCreationExpression(
            this Type implementationType,
            Expression baseExpression,
            params Expression[] interceptors)
        {
            var methodName = implementationType.IsInterface
                ? nameof(ProxyGenerator.CreateInterfaceProxyWithTarget)
                : nameof(ProxyGenerator.CreateClassProxyWithTarget);

            return CreateProxyExpression(implementationType, baseExpression, methodName, interceptors);
        }

        private static Expression CreateProxyExpression(
            Type implementationType,
            Expression baseExpression,
            string proxyMethodName,
            Expression[] interceptors)
        {
            var type = typeof(ProxyGenerator);
            var proxyGeneratorNewExpression = Expression.New(type);

            var createProxyMethod = type.GetMethods()
                .FirstOrDefault(x => x.Name == proxyMethodName &&
                                     x.GetParameters().Length == 3 &&
                                     x.GetParameters().Skip(1).First().ParameterType == typeof(object));

            if (createProxyMethod == null)
            {
                throw new MissingMethodException($"Method {proxyMethodName} not found!");
            }

            var newArrayExpression = Expression.NewArrayInit(typeof(IInterceptor), interceptors);
            var implementationTypeExpression = Expression.Constant(implementationType);
            var createProxyExpression =
                Expression.Call(proxyGeneratorNewExpression,
                    createProxyMethod,
                    implementationTypeExpression,
                    baseExpression,
                    newArrayExpression);
            return Expression.Convert(createProxyExpression, implementationType);
        }

        private static bool ImplementationsOfRequestedTypeHasTransactionalAttribute(Type type)
        {
            var implementationsOfRequestedTypeHasTransactionalAttribute = type.IsInterface && type.Assembly.GetTypes()
                .Where(x => !x.IsInterface && x.GetInterfaces().Any(i => i == type))
                .SelectMany(x => x.GetMethods())
                .Any(x => x.GetCustomAttribute<TransactionalAttribute>() != null);
            return implementationsOfRequestedTypeHasTransactionalAttribute;
        }

        private static bool RequestedTypeHasTransactionalAttribute(Type type)
        {
            var requestedTypeHasTransactionalAttribute = type.GetMethods()
                .Any(x => x.GetCustomAttribute<TransactionalAttribute>() != null);
            return requestedTypeHasTransactionalAttribute;
        }
    }
}