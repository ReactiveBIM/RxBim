namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Di;
    using Models.Configurations;
    using Shared;

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
            if (methodName == Constants.StartMethodName)
            {
                try
                {
                    var ribbonMenuBuilder = container.GetService<IRibbonMenuBuilder>();
                    var ribbonConfiguration = container.GetService<Ribbon>();
                    ribbonMenuBuilder.BuildRibbonMenu(ribbonConfiguration);
                }
                catch (Exception e)
                {
                    throw new MethodCallerException("Failed to build ribbon", e);
                }
            }

            return Decorated.InvokeMethod(container, methodName);
        }
    }
}