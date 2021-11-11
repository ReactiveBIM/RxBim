namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using Models.Configurations;

    /// <summary>
    /// Ribbon builder
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Creates tab and adds to the ribbon
        /// </summary>
        /// <param name="tabTitle">Tab title</param>
        ITabBuilder AddTab(string tabTitle);

        /// <summary>
        /// Sets value for <see cref="Models.Configurations.Ribbon.AddVersionToCommandTooltip"/>
        /// </summary>
        /// <param name="value">Value</param>
        IRibbonBuilder SetAddVersionToCommandTooltip(bool value);

        /// <summary>
        /// Sets value for <see cref="Ribbon.CommandTooltipVersionHeader"/>
        /// </summary>
        /// <param name="header">Value</param>
        IRibbonBuilder SetCommandTooltipVersionHeader(string header);
    }
}