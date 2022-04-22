namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Defines a ribbon builder.
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Adds a new tab to the ribbon.
        /// </summary>
        /// <param name="title">The tab title text.</param>
        /// <param name="tab">The tab configurator.</param>
        IRibbonBuilder AddTab(string title, Action<ITabBuilder> tab);

        /// <summary>
        /// Sets whether to add the version information to the tooltip text.
        /// </summary>
        /// <param name="enable">The value.</param>
        IRibbonBuilder SetDisplayVersion(bool enable);

        /// <summary>
        /// Sets the version prefix.
        /// </summary>
        /// <param name="prefix">The prefix text.</param>
        IRibbonBuilder SetVersionPrefix(string prefix);
    }
}