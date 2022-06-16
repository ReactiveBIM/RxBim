namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.UI;
    using Shared.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for stacked items.
    /// </summary>
    public class StackedItemsStrategy : ItemStrategyBase<StackedItems>
    {
        private readonly IDiCollectionService<IAddItemStrategy> _strategiesService;

        /// <inheritdoc />
        public StackedItemsStrategy(IDiCollectionService<IAddItemStrategy> strategiesService, MenuData menuData)
            : base(menuData)
        {
            _strategiesService = strategiesService;
        }

        /// <inheritdoc />
        protected override void AddItem(string tabName, RibbonPanel ribbonPanel, StackedItems stackedItems)
        {
            var strategies = _strategiesService.GetItems().ToList();

            var button1 = GetStackedItem(strategies, stackedItems.Items[0]);
            var button2 = GetStackedItem(strategies, stackedItems.Items[1]);

            switch (stackedItems.Items.Count)
            {
                case 2:
                    ribbonPanel.AddStackedItems(button1, button2);
                    break;
                case 3:
                    var button3 = GetStackedItem(strategies, stackedItems.Items[2]);
                    ribbonPanel.AddStackedItems(button1, button2, button3);
                    break;
                default:
                    throw new InvalidOperationException("The stack size can only be 2 or 3!");
            }
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(StackedItems itemConfig)
        {
            return CannotBeStackedStub(itemConfig);
        }

        private static RibbonItemData GetStackedItem(
            IEnumerable<IAddItemStrategy> strategies,
            IRibbonPanelItem firstItem)
        {
            var strategy = strategies.FirstOrDefault(x => x.IsApplicable(firstItem));
            if (strategy is null)
                throw new InvalidOperationException($"Can't found strategy for: {firstItem.GetType().FullName}");
            return (RibbonItemData)strategy.GetItemForStack(firstItem);
        }
    }
}