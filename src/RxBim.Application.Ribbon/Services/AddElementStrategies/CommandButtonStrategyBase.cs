﻿namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

    /// <summary>
    /// Base implementation of <see cref="IAddElementStrategy"/> for command button.
    /// </summary>
    public abstract class CommandButtonStrategyBase : IAddElementStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IRibbonPanelElement config)
        {
            return config is CommandButton;
        }

        /// <inheritdoc />
        public abstract void CreateAndAddElement(object panel, IRibbonPanelElement config);

        /// <inheritdoc />
        public abstract object CreateElementForStack(IRibbonPanelElement config, bool small);
    }
}