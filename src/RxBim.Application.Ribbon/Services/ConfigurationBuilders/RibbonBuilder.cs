namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

    /// <inheritdoc />
    public class RibbonBuilder : IRibbonBuilder
    {
        /// <summary>
        /// Building ribbon
        /// </summary>
        public Ribbon Ribbon { get; } = new ();

        /// <inheritdoc />
        public ITabBuilder AddTab(string tabTitle)
        {
            var builder = new TabBuilder(tabTitle, this);
            Ribbon.Tabs.Add(builder.BuildingTab);
            return builder;
        }
    }
}