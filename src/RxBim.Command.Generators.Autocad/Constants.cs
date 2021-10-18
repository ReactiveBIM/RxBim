namespace RxBim.Command.Generators.Autocad
{
    /// <summary>
    /// Constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Full name of the command class attribute type
        /// </summary>
        public const string CommandClassAttributeTypeFullName =
            "RxBim.Command.Autocad.RxBimCommandClassAttribute";

        /// <summary>
        /// Название базового класса команды
        /// </summary>
        public const string BaseCommandClassName = "RxBimCommand";

        /// <summary>
        /// Название типа для командных флагов
        /// </summary>
        public const string CommandFlags = nameof(CommandFlags);

        /// <summary>
        /// Значение командного флага по умолчанию
        /// </summary>
        public const string DefaultCommandFlag = CommandFlags + ".Modal";

        /// <summary>
        /// Разделитель флагов
        /// </summary>
        public const string FlagsSeparator = " | ";

        /// <summary>
        /// Файл с кодом для генерации класса
        /// </summary>
        public const string CommandClassSource = "CommandClass.source";

        /// <summary>
        /// Добавка в название для сгенерированного
        /// </summary>
        public const string Generated = nameof(Generated);
    }
}