namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using System.Collections.Generic;
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;

    public class CommandButtonStrategy : ElementFromConfigStrategyBase
    {
        /// <inheritdoc />
        public override bool IsApplicable(IConfigurationSection elementSection)
        {
            return elementSection.GetSection(nameof(CommandButton.CommandType)).Exists();
        }

        /// <inheritdoc />
        public override void CreateFromConfigAndAdd(
            IConfigurationSection elementSection,
            ICollection<IRibbonPanelElement> elements)
        {
            CreateSimpleFromConfigAndAddInternal<CommandButton>(elementSection, elements);
        }
    }
}