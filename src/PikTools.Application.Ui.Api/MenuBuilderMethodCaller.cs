namespace PikTools.Application.Ui.Api
{
    using System;
    using Autodesk.Revit.UI;
    using Builder;
    using Di;
    using SimpleInjector;

    /// <summary>
    /// Декоратор, строящий ленту плагина
    /// </summary>
    /// <typeparam name="T">тип возвращаемого значения</typeparam>
    public class MenuBuilderMethodCaller<T> : IMethodCaller<T>
    {
        private readonly IMethodCaller<T> _decorated;
        private readonly UIControlledApplication _controlledApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="decorated">Декорируемый объект</param>
        /// <param name="controlledApp">UIControlledApplication</param>
        public MenuBuilderMethodCaller(IMethodCaller<T> decorated, UIControlledApplication controlledApp)
        {
            _decorated = decorated;
            _controlledApp = controlledApp;
        }

        /// <inheritdoc />
        public Type SourceObjectType => _decorated.SourceObjectType;

        /// <inheritdoc />
        public T InvokeCommand(Container container, string methodName)
        {
            try
            {
                var ribbonBuilder = container.GetInstance<Action<Ribbon>>();
                ribbonBuilder(new Ribbon(_controlledApp));
            }
            catch (Exception e)
            {
                throw new MethodCallerException("Не удалось построить ленту", e);
            }

            return _decorated.InvokeCommand(container, methodName);
        }
    }
}