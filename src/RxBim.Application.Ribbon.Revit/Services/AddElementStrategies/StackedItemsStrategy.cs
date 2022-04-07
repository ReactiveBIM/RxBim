namespace RxBim.Application.Ribbon.Revit.Services.AddElementStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Autodesk.Revit.UI;
    using Di;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for stacked items.
    /// </summary>
    public class StackedItemsStrategy : ElementStrategyBase<StackedItems>
    {
        private readonly IServiceLocator _serviceLocator;

        /// <inheritdoc />
        public StackedItemsStrategy(IServiceLocator serviceLocator, MenuData menuData)
            : base(menuData)
        {
            _serviceLocator = serviceLocator;
        }

        /// <inheritdoc />
        protected override void CreateAndAddElement(string tabName, RibbonPanel ribbonPanel, StackedItems stackedItems)
        {
            var strategies = _serviceLocator.GetServicesAssignableTo<IAddElementStrategy>().ToList();

            var button1 = GetStackedItem(strategies, stackedItems.StackedElements[0]);
            var button2 = GetStackedItem(strategies, stackedItems.StackedElements[1]);

            switch (stackedItems.StackedElements.Count)
            {
                case 2:
                    ribbonPanel.AddStackedItems(button1, button2);
                    break;
                case 3:
                    var button3 = GetStackedItem(strategies, stackedItems.StackedElements[2]);
                    ribbonPanel.AddStackedItems(button1, button2, button3);
                    break;
                default:
                    throw new InvalidOperationException("The stack size can only be 2 or 3!");
            }
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateElementForStack(StackedItems elementConfig)
        {
            return CannotBeStackedStub(elementConfig);
        }

        private static RibbonItemData GetStackedItem(
            IEnumerable<IAddElementStrategy> strategies,
            IRibbonPanelElement firstItem)
        {
            var strategy = strategies.FirstOrDefault(x => x.IsApplicable(firstItem));
            if (strategy is null)
                throw new InvalidOperationException($"Can't found strategy for: {firstItem.GetType().FullName}");
            return (RibbonItemData)strategy.CreateElementForStack(firstItem);
        }
    }
}