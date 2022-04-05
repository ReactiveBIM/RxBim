namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using System;
    using Abstractions;
    using Autodesk.Windows;
    using Ribbon.Abstractions;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Services.AddElementStrategies;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : SeparatorStrategyBase
    {
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public SeparatorStrategy(IPanelService panelService)
        {
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override void CreateAndAddElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel)
                return;

            _panelService.AddSeparator(ribbonPanel);
        }

        /// <inheritdoc />
        public override object CreateElementForStack(IRibbonPanelElement config, bool small)
        {
            throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");
        }
    }
}