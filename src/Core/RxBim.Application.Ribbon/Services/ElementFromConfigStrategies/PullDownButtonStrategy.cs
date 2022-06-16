namespace RxBim.Application.Ribbon.ElementFromConfigStrategies
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a <see cref="PullDownButton"/> from a configuration section.
    /// </summary>
    public class PullDownButtonStrategy : SimpleElementStrategyBase<PullDownButton>
    {
        /// <inheritdoc />
        public override bool IsApplicable(IConfigurationSection elementSection)
        {
            return elementSection.GetSection(nameof(PullDownButton.CommandButtonsList)).Exists();
        }
    }
}