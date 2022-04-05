namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;

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