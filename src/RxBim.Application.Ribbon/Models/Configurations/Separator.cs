namespace RxBim.Application.Ribbon.Models.Configurations
{
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Ribbon panel elements separator
    /// </summary>
    public class Separator : IRibbonPanelElement
    {
        /// <summary>
        /// Separator type
        /// </summary>
        public SeparatorType SeparatorType { get; set; }
    }
}