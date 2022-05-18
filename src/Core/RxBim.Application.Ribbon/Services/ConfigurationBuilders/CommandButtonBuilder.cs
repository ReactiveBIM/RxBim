namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;

    /// <summary>
    /// Represents a button builder.
    /// </summary>
    public class CommandButtonBuilder : ButtonBuilder<CommandButton, ICommnadButtonBuilder>, ICommnadButtonBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandButtonBuilder"/> class.
        /// </summary>
        /// <param name="name">The button name.</param>
        /// <param name="commandType">The command type.</param>
        public CommandButtonBuilder(string name, Type commandType)
            : base(name)
        {
            ////CommandType = commandType;
            BuildingButton.CommandType = commandType.AssemblyQualifiedName;
        }
    }
}