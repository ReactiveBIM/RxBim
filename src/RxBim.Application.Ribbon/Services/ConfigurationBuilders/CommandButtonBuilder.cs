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
        private readonly Type _commandType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandButtonBuilder"/> class.
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="commandType">Command class type</param>
        public CommandButtonBuilder(string name, Type commandType)
            : base(name)
        {
            _commandType = commandType;
            BuildingButton.CommandType = commandType.FullName;
        }

        /// <inheritdoc />
        public ICommandButtonBuilder SetToolTip(
            string toolTip,
            bool addVersion = true,
            string versionInfoHeader = "")
        {
            if (addVersion)
            {
                if (toolTip.Length > 0)
                    toolTip += Environment.NewLine;
                toolTip += $"{versionInfoHeader}{_commandType.Assembly.GetName().Version}";
            }

            BuildingButton.ToolTip = toolTip;

            return this;
        }

        /// <inheritdoc />
        public ICommandButtonBuilder SetHelpUrl(string url)
        {
            BuildingButton.HelpUrl = url;
            return this;
        }
    }
}