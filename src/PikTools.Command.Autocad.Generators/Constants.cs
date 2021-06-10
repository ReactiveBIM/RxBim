namespace PikTools.Command.Autocad.Generators
{
    /// <summary>
    /// Константы
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Полное название типа атрибута для командного класса
        /// </summary>
        public const string CommandClassAttributeTypeFullName =
            "PikTools.Command.Autocad.Api.PikToolsCommandClassAttribute";

        /// <summary>
        /// Название базового класса команды
        /// </summary>
        public const string BaseCommandClassName = "PikToolsCommand";

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