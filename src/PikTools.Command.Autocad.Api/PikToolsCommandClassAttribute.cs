namespace PikTools.Command.Autocad.Api
{
    using System;
    using Autodesk.AutoCAD.Runtime;

    /// <summary>
    /// Атрибут для настройки команды
    /// </summary>
    public class PikToolsCommandClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PikToolsCommandClassAttribute"/> class.
        /// </summary>
        /// <param name="flags">Флаги команды</param>
        public PikToolsCommandClassAttribute(CommandFlags flags)
            : this(string.Empty, flags)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PikToolsCommandClassAttribute"/> class.
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="flags">Флаги команды</param>
        public PikToolsCommandClassAttribute(string commandName, CommandFlags flags = CommandFlags.Modal)
        {
            CommandName = commandName;
            Flags = flags;
        }

        /// <summary>
        /// Имя команды. Если не задано - используется название класса.
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// Флаги команды
        /// </summary>
        public CommandFlags Flags { get; }
    }
}