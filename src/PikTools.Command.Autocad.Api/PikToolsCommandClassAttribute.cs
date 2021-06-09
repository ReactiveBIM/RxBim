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
        public PikToolsCommandClassAttribute()
        {
            Flags = CommandFlags.Modal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PikToolsCommandClassAttribute"/> class.
        /// </summary>
        /// <param name="flags">Флаги команды</param>
        public PikToolsCommandClassAttribute(CommandFlags flags)
        {
            Flags = flags;
        }

        /// <summary>
        /// Флаги команды
        /// </summary>
        public CommandFlags Flags { get; }
    }
}