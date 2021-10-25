namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

    /// <summary>
    /// Command button builder
    /// </summary>
    public class CommandButtonBuilder : ButtonBuilder<CommandButton>, ICommandButtonBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandButtonBuilder"/> class.
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="commandType">Command class type</param>
        public CommandButtonBuilder(string name, Type commandType)
            : base(name)
        {
            CommandType = commandType;
            BuildingButton.CommandType = commandType.FullName;
        }

        /// <summary>
        /// Command class type
        /// </summary>
        public Type CommandType { get; }

        /// <inheritdoc />
        public ICommandButtonBuilder SetToolTip(
            string toolTip,
            bool addVersion = true,
            string versionInfoHeader = "")
        {
            BuildingButton.ToolTip = toolTip;
            BuildingButton.ToolTipSettings.AddVersion = addVersion;
            BuildingButton.ToolTipSettings.VersionHeader = versionInfoHeader;

            return this;
        }
    }
}