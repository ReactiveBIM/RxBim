namespace RxBim.Application.Ribbon
{
    using Di;
    using Shared;

    /// <summary>
    /// Decorator creating the plugin ribbon.
    /// </summary>
    /// <typeparam name="T">The return type of the method.</typeparam>
    public class MenuBuilderMethodCaller<T> : MethodCallerDecorator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBuilderMethodCaller{T}"/> class.
        /// </summary>
        /// <param name="decorated">Decorated object.</param>
        public MenuBuilderMethodCaller(IMethodCaller<T> decorated)
            : base(decorated)
        {
        }

        /// <inheritdoc />
        public override T InvokeMethod(IContainer container, string methodName)
        {
            if (methodName == Constants.StartMethodName)
                container.GetRequiredService<IServiceLocator>().BuildRibbonMenu();

            return Decorated.InvokeMethod(container, methodName);
        }
    }
}