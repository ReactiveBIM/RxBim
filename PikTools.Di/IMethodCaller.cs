namespace PikTools.Di
{
    using SimpleInjector;

    /// <summary>
    /// MethodCaller
    /// </summary>
    public interface IMethodCaller<T>
    {
        /// <summary>
        /// Вызывает комманду
        /// </summary>
        /// <param name="container">конетйнер</param>
        /// <param name="methodName">имя метода</param>
        T InvokeCommand(Container container, string methodName);
    }
}