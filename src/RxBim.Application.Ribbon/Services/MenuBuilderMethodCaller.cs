namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Di;

    /// <summary>
    /// Decorator creating the plugin ribbon
    /// </summary>
    /// <typeparam name="T">The return type of the method</typeparam>
    public class MenuBuilderMethodCaller<T> : MethodCallerDecorator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuBuilderMethodCaller{T}"/> class.
        /// </summary>
        /// <param name="decorated">Decorated object</param>
        public MenuBuilderMethodCaller(IMethodCaller<T> decorated)
            : base(decorated)
        {
        }

        /// <inheritdoc />
        public override T InvokeMethod(IContainer container, string methodName)
        {
            try
            {
                var menuBuilder = container.GetService<IMenuBuildService>();
                menuBuilder.BuildMenu(container);
            }
            catch (Exception e)
            {
                throw new MethodCallerException("Failed to build ribbon", e);
            }

            return Decorated.InvokeMethod(container, methodName);
        }
    }
}