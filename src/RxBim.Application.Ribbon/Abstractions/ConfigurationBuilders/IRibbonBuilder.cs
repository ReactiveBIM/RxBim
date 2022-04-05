namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using Models.Configurations;

    /// <summary>
    /// The bibbon builder.
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Creates tab and adds to the ribbon.
        /// </summary>
        /// <param name="tabTitle">Tab title.</param>
        ITabBuilder AddTab(string tabTitle);

        /// <summary>
        /// Sets a value for <see cref="Models.Configurations.Ribbon.AddVersionToCommandTooltip"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        IRibbonBuilder SetAddVersionToCommandTooltip(bool value);

        /// <summary>
        /// Sets a value for <see cref="Ribbon.CommandTooltipVersionHeader"/>.
        /// </summary>
        /// <param name="header">The value.</param>
        IRibbonBuilder SetCommandTooltipVersionHeader(string header);
    }
}