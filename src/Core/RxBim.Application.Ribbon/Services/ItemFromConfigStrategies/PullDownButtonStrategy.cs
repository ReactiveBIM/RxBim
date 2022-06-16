namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a <see cref="PullDownButton"/> from a configuration section.
    /// </summary>
    public class PullDownButtonStrategy : SimpleItemStrategyBase<PullDownButton>
    {
        /// <inheritdoc />
        public override bool IsApplicable(IConfigurationSection itemSection)
        {
            return itemSection.GetSection(nameof(PullDownButton.CommandButtonsList)).Exists();
        }
    }
}