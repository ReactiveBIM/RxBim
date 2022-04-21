namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Defines a ribbon builder.
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Adds a new tab to the ribbon.
        /// </summary>
        /// <param name="title">The tab title text.</param>
        ITabBuilder AddTab(string title);

        /// <summary>
        /// Sets whether to add the version information to the tooltip text.
        /// </summary>
        /// <param name="enable">The value.</param>
        IRibbonBuilder SetAddVersionToCommandTooltip(bool enable);

        /// <summary>
        /// Sets the version prefix.
        /// </summary>
        /// <param name="prefix">The prefix text.</param>
        IRibbonBuilder SetCommandTooltipVersionHeader(string prefix);
    }
}