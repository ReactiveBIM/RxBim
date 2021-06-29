namespace PikTools.Shared.AutocadExtensions.Services
{
    using Abstractions;
    using Autodesk.AutoCAD.EditorInput;

    /// <summary>
    /// Сервис для работы с командной строкой
    /// </summary>
    public class CommandLineService : ICommandLineService
    {
        private readonly Editor _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineService"/> class.
        /// </summary>
        /// <param name="editor">Редактор документа</param>
        public CommandLineService(Editor editor)
        {
            _editor = editor;
        }

        /// <summary>
        /// Выводит текстовое сообщение с новой строки
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void WriteAsNewLine(string message)
        {
            _editor.WriteMessage($"\n{message}");
        }
    }
}