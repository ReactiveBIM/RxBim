namespace RxBim.Application.Menu.Fluent.Revit.Sample
{
    using Ribbon.Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Extensions for <see cref="ICommandButtonBuilder"/>
    /// </summary>
    internal static class CommandButtonBuilderExtensions
    {
        /// <summary>
        /// Sets the tooltip with the same settings
        /// </summary>
        /// <param name="builder">Command button builder</param>
        /// <param name="tooltip">Tooltip text</param>
        /// <returns>Builder from '<paramref name="builder"/>' parameter</returns>
        public static ICommandButtonBuilder SetToolTipWithGlobalSettings(
            this ICommandButtonBuilder builder,
            string tooltip)
        {
            return builder.SetToolTip(tooltip, true, "Version: ");
        }
    }
}