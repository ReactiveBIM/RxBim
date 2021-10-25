namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using Models.Configurations;

    /// <summary>
    /// Command button builder
    /// </summary>
    public interface ICommandButtonBuilder : IButtonBuilder
    {
        /// <summary>
        /// Sets a tooltip for the button
        /// </summary>
        /// <param name="toolTip">Tooltip text</param>
        /// <param name="addVersion"><see cref="CommandButtonToolTipSettings.AddVersion"/></param>
        /// <param name="versionHeader"><see cref="CommandButtonToolTipSettings.VersionHeader"/></param>
        ICommandButtonBuilder SetToolTip(string toolTip, bool addVersion = true, string versionHeader = "");
    }
}