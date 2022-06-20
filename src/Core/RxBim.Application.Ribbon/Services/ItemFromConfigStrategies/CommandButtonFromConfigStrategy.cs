namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a <see cref="CommandButton"/> from a configuration section.
    /// </summary>
    public class CommandButtonFromConfigStrategy : SimpleItemFromConfigStrategyBase<CommandButton>
    {
        /// <inheritdoc />
        public override bool IsApplicable(IConfigurationSection itemSection)
        {
            return itemSection.GetSection(nameof(CommandButton.CommandType)).Exists();
        }
    }
}