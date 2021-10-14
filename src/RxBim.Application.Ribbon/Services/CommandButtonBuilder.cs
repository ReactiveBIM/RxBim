namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Models;

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
        /// <param name="text">Button label text</param>
        /// <param name="commandType">Command class type</param>
        public CommandButtonBuilder(string name, string text, Type commandType)
            : base(name, text)
        {
            _commandType = commandType;
            Control.CommandType = commandType.FullName;
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

            Control.ToolTip = toolTip;

            return this;
        }

        /// <inheritdoc />
        public ICommandButtonBuilder SetHelpUrl(string url)
        {
            Control.HelpUrl = url;
            return this;
        }
    }
}