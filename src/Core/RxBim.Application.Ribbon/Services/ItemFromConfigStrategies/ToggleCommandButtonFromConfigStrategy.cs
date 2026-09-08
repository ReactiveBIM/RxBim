namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a <see cref="ToggleCommandButton"/> from a configuration section.
    /// </summary>
    public class ToggleCommandButtonFromConfigStrategy : SimpleItemFromConfigStrategyBase<ToggleCommandButton>
    {
        /// <inheritdoc />
        public override bool IsApplicable(IConfigurationSection itemSection)
        {
            return itemSection.GetSection(nameof(ToggleCommandButton.CommandType)).Exists()
                   && itemSection.GetSection(nameof(ToggleCommandButton.IsToggle)).Exists();
        }
    }
}