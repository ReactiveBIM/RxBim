namespace RxBim.Application.Ribbon.Services
{
    using Abstractions;
    using Models;

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
            Ribbon.Tabs.Add(builder.Control);
            return builder;
        }
    }
}