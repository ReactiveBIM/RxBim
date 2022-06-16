namespace RxBim.Application.Ribbon.ElementFromConfigStrategies
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

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