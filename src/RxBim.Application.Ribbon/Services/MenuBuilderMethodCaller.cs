namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Di;

    /// <summary>
    /// Декоратор, строящий ленту плагина
    /// </summary>
    /// <typeparam name="T">тип возвращаемого значения</typeparam>
    public class MenuBuilderMethodCaller<T> : MethodCallerDecorator<T>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="decorated">Декорируемый объект</param>
        public MenuBuilderMethodCaller(IMethodCaller<T> decorated)
            : base(decorated)
        {
        }

        /// <inheritdoc />
        public override T InvokeCommand(IContainer container, string methodName)
        {
            try
            {
                var menuBuilder = container.GetService<IMenuBuildService>();
                menuBuilder.BuildMenu(container);
            }
            catch (Exception e)
            {
                throw new MethodCallerException("Не удалось построить ленту", e);
            }

            return Decorated.InvokeCommand(container, methodName);
        }
    }
}