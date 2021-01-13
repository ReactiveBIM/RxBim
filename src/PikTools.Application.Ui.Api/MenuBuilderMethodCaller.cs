namespace PikTools.Application.Ui.Api
{
    using System;
    using Autodesk.Revit.UI;
    using Builder;
    using Di;

    /// <summary>
    /// Декоратор, строящий ленту плагина
    /// </summary>
    /// <typeparam name="T">тип возвращаемого значения</typeparam>
    public class MenuBuilderMethodCaller<T> : MethodCallerDecorator<T>
    {
        private readonly UIControlledApplication _controlledApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="decorated">Декорируемый объект</param>
        /// <param name="controlledApp">UIControlledApplication</param>
        public MenuBuilderMethodCaller(IMethodCaller<T> decorated, UIControlledApplication controlledApp)
            : base(decorated)
        {
            _controlledApp = controlledApp;
        }

        /// <inheritdoc />
        public override T InvokeCommand(IContainer container, string methodName)
        {
            try
            {
                var ribbonBuilder = container.GetService<Action<Ribbon>>();
                ribbonBuilder(new Ribbon(_controlledApp));
            }
            catch (Exception e)
            {
                throw new MethodCallerException("Не удалось построить ленту", e);
            }

            return Decorated.InvokeCommand(container, methodName);
        }
    }
}