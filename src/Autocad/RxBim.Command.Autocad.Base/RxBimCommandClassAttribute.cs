namespace RxBim.Command.Autocad.Base
{
    using Autodesk.AutoCAD.Runtime;
    using Shared;

    /// <summary>
    /// Autocad command attribute.
    /// </summary>
    public class RxBimCommandClassAttribute : RxBimCommandAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RxBimCommandClassAttribute"/> class.
        /// </summary>
        /// <param name="flags">The command flags.</param>
        public RxBimCommandClassAttribute(CommandFlags flags)
            : this(string.Empty, flags)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RxBimCommandClassAttribute"/> class.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="flags">The command flags.</param>
        public RxBimCommandClassAttribute(string commandName, CommandFlags flags = CommandFlags.Modal)
        {
            CommandName = commandName;
            Flags = flags;
        }

        /// <summary>
        /// Command name. By default uses command class name.
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// The command flags.
        /// </summary>
        public CommandFlags Flags { get; }
    }
}