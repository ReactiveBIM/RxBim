﻿namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System;
    using System.Linq;
    using Autodesk.Windows;
    using ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for stacked items.
    /// </summary>
    public class StackedItemsStrategy : ItemStrategyBase<StackedItems>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public StackedItemsStrategy(IServiceProvider serviceProvider, IPanelService panelService)
        {
            _serviceProvider = serviceProvider;
            _panelService = panelService;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, StackedItems stackedItems)
        {
            var stackSize = stackedItems.Items.Count;
            var stackedItemsRow = new RibbonRowPanel();
            var small = stackSize == StackedItemsBuilder.MaxStackSize;

            var strategies = _serviceProvider.GetServices<IItemStrategy>().ToList();

            _panelService.AddItem(ribbonPanel, stackedItemsRow);

            for (var i = 0; i < stackSize; i++)
            {
                if (i > 0)
                    stackedItemsRow.Items.Add(new RibbonRowBreak());

                var buttonConfig = stackedItems.Items[i];

                var itemStrategy = strategies.FirstOrDefault(x => x.IsApplicable(buttonConfig));
                if (itemStrategy is null)
                    continue;

                var item = (RibbonItem)itemStrategy.GetItemForStack(buttonConfig, small);
                stackedItemsRow.Items.Add(item);
            }
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(StackedItems itemConfig, RibbonItemSize size)
        {
            return CantBeStackedStub(itemConfig);
        }
    }
}