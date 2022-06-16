﻿namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using System;
    using System.Linq;
    using Application.Ribbon.AddElementStrategies;
    using Autodesk.Windows;
    using ConfigurationBuilders;
    using Shared.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for stacked items.
    /// </summary>
    public class StackedItemsStrategy : StackedItemStrategyBase
    {
        private readonly IStrategyFactory<IAddElementStrategy> _strategyFactory;
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public StackedItemsStrategy(IStrategyFactory<IAddElementStrategy> strategyFactory, IPanelService panelService)
        {
            _strategyFactory = strategyFactory;
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override void CreateElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not StackedItems stackedItems)
                return;

            var stackSize = stackedItems.StackedButtons.Count;
            var stackedItemsRow = new RibbonRowPanel();
            var small = stackSize == StackedItemsBuilder.MaxStackSize;

            var strategies = _strategyFactory.GetStrategies().ToList();

            _panelService.AddItem(ribbonPanel, stackedItemsRow);

            for (var i = 0; i < stackSize; i++)
            {
                if (i > 0)
                    stackedItemsRow.Items.Add(new RibbonRowBreak());

                var buttonConfig = stackedItems.StackedButtons[i];

                var addElementStrategy = strategies.FirstOrDefault(x => x.IsApplicable(buttonConfig));
                if (addElementStrategy is null)
                    continue;

                var element = (RibbonItem)addElementStrategy.CreateElementForStack(buttonConfig, small);
                stackedItemsRow.Items.Add(element);
            }
        }

        /// <inheritdoc />
        public override object CreateElementForStack(IRibbonPanelElement config, bool small)
        {
            throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");
        }
    }
}