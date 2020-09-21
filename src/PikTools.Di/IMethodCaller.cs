namespace PikTools.Di
{
    using System;
    using SimpleInjector;

    /// <summary>
    /// MethodCaller
    /// </summary>
    public interface IMethodCaller<T>
    {
        /// <summary>
        /// Тип объекта у которого вызыается метод
        /// </summary>
        public Type SourceObjectType { get; }

        /// <summary>
        /// Вызывает комманду
        /// </summary>
        /// <param name="container">конетйнер</param>
        /// <param name="methodName">имя метода</param>
        T InvokeCommand(Container container, string methodName);
    }
}