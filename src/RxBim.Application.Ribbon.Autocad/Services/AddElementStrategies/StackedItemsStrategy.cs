namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using System.Linq;
    using Abstractions;
    using Autodesk.Windows;
    using Di;
    using Models.Configurations;
    using Ribbon.Abstractions;
    using Ribbon.Services.ConfigurationBuilders;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for stacked items.
    /// </summary>
    public class StackedItemsStrategy : ElementStrategyBase<StackedItems>
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public StackedItemsStrategy(IServiceLocator serviceLocator, IPanelService panelService)
        {
            _serviceLocator = serviceLocator;
            _panelService = panelService;
        }

        /// <inheritdoc />
        protected override void CreateAndAddElement(
            RibbonTab ribbonTab,
            RibbonPanel ribbonPanel,
            StackedItems stackedItems)
        {
            var stackSize = stackedItems.StackedElements.Count;
            var stackedItemsRow = new RibbonRowPanel();
            var small = stackSize == StackedItemsBuilder.MaxStackSize;

            var strategies = _serviceLocator.GetServicesAssignableTo<IAddElementStrategy>().ToList();

            _panelService.AddItem(ribbonPanel, stackedItemsRow);

            for (var i = 0; i < stackSize; i++)
            {
                if (i > 0)
                    stackedItemsRow.Items.Add(new RibbonRowBreak());

                var buttonConfig = stackedItems.StackedElements[i];

                var addElementStrategy = strategies.FirstOrDefault(x => x.IsApplicable(buttonConfig));
                if (addElementStrategy is null)
                    continue;

                var element = (RibbonItem)addElementStrategy.CreateElementForStack(buttonConfig, small);
                stackedItemsRow.Items.Add(element);
            }
        }

        /// <inheritdoc />
        protected override RibbonItem CreateElementForStack(StackedItems elementConfig, RibbonItemSize size)
        {
            return CantBeStackedStub(elementConfig);
        }
    }
}