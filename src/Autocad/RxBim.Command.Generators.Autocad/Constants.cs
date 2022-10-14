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
            "RxBim.Command.Autocad.RxBimCommandClassAttribute";

        /// <summary>
        /// Base command class name.
        /// </summary>
        public const string BaseCommandClassName = "RxBimCommand";

        /// <summary>
        /// Command flags type name.
        /// </summary>
        public const string CommandFlags = nameof(CommandFlags);

        /// <summary>
        /// Default command flags value.
        /// </summary>
        public const string DefaultCommandFlag = CommandFlags + ".Modal";

        /// <summary>
        /// Command flags separator.
        /// </summary>
        public const string FlagsSeparator = " | ";

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