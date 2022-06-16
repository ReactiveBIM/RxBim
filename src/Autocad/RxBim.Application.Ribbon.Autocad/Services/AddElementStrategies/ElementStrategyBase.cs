namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using System;
    using Autodesk.Windows;

    /// <summary>
    /// Basic implementation of <see cref="IAddElementStrategy"/> for AutoCAD menu item.
    /// </summary>
    public abstract class ElementStrategyBase<T> : IAddElementStrategy
        where T : IRibbonPanelElement
    {
        /// <inheritdoc />
        public virtual bool IsApplicable(IRibbonPanelElement config)
        {
            return config is T;
        }

        /// <inheritdoc />
        public void CreateAndAddElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not T elementConfig)
                return;

            CreateAndAddElement(ribbonPanel, elementConfig);
        }

        /// <inheritdoc />
        public object CreateElementForStack(IRibbonPanelElement config, bool small = false)
        {
            if (config is not T elementConfig)
                throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");
            var size = small ? RibbonItemSize.Standard : RibbonItemSize.Large;

            return CreateElementForStack(elementConfig, size);
        }

        /// <summary>
        /// Creates and adds to ribbon an element.
        /// </summary>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="elementConfig">Ribbon item configuration.</param>
        protected abstract void CreateAndAddElement(RibbonPanel ribbonPanel, T elementConfig);

        /// <summary>
        /// Creates and returns an element for a stack.
        /// </summary>
        /// <param name="elementConfig">Ribbon item configuration.</param>
        /// <param name="size">Item size.</param>
        protected abstract RibbonItem CreateElementForStack(T elementConfig, RibbonItemSize size);

        /// <summary>
        /// Stub for CreateElementForStack, if element can't be stacked.
        /// </summary>
        /// <param name="elementConfig">Ribbon item configuration.</param>
        protected RibbonItem CantBeStackedStub(T elementConfig)
        {
            throw new InvalidOperationException($"Can't be stacked: {elementConfig.GetType().FullName}");
        }
    }
}