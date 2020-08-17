namespace PikTools.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using SimpleInjector;

    /// <summary>
    /// MethodCaller
    /// </summary>
    public class MethodCaller<T> : IMethodCaller<T>
    {
        private readonly object _sourceObject;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="sourceObject">Тип вызываемой комманды</param>
        public MethodCaller(object sourceObject)
        {
            _sourceObject = sourceObject;
        }

        /// <inheritdoc />
        public Type SourceObjectType => _sourceObject.GetType();

        /// <summary>
        /// Вызывает комманду
        /// </summary>
        /// <param name="container">конетйнер</param>
        /// <param name="methodName">имя метода</param>
        public T InvokeCommand(Container container, string methodName)
        {
            var methodInfo = FindInvokeMethod(methodName);

            if (!methodInfo.GetParameters().Any())
            {
                return GetResult(methodInfo);
            }

            var parameters = GetMethodParameters(container, methodInfo);
            return GetResult(methodInfo, parameters.ToArray());
        }

        private T GetResult(
            MethodInfo methodInfo,
            object[] parameters = null)
        {
            return (T)methodInfo.Invoke(_sourceObject, parameters);
        }

        private List<object> GetMethodParameters(Container container, MethodInfo methodInfo)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                parameters.Add(container.GetInstance(parameterInfo.ParameterType));
            }

            return parameters;
        }

        private MethodInfo FindInvokeMethod(string methodName)
        {
            var type = _sourceObject.GetType();
            var methodInfo = type.GetMethods().SingleOrDefault(x => x.Name.ToLower() == methodName.ToLower());
            if (methodInfo == null)
            {
                throw new MethodCallerException($"Класс {type.FullName} не содержит метода {methodName}!");
            }

            if (methodInfo.ReturnType != typeof(T))
            {
                throw new MethodCallerException(
                    $"Метод {methodName} имеет тип возвращаемого значения отличный от {typeof(T)}!");
            }

            return methodInfo;
        }
    }
}