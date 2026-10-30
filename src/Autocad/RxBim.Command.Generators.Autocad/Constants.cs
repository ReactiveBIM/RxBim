namespace RxBim.Command.Generators.Autocad
{
    /// <summary>
    /// Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Full name of the command class attribute type.
        /// </summary>
        public const string CommandClassAttributeTypeFullName =
            "RxBim.Shared.Autocad.RxBimCommandClassAttribute";

        /// <summary>
        /// Base command class name.
        /// </summary>
        public const string BaseCommandClassName = "RxBimCommand";

        /// <summary>
        /// Command name constructor parameter name.
        /// </summary>
        public const string CommandNameParameter = "commandName";

        /// <summary>
        /// Command flags constructor parameter name.
        /// </summary>
        public const string CommandFlagsParameter = "flags";

        /// <summary>
        /// Command flags type name.
        /// </summary>
        public const string CommandFlags = nameof(CommandFlags);

        /// <summary>
        /// Default command flags value.
        /// </summary>
        public const string DefaultCommandFlag = CommandFlags + ".Modal";

        /// <summary>
        /// Command class source name.
        /// </summary>
        public const string CommandClassSource = "CommandClass.source";

        /// <summary>
        /// Suffix for generated class.
        /// </summary>
        public const string Generated = nameof(Generated);
    }
}