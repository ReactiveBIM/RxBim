namespace PikTools.Shared.AutocadExtensions.Services
{
    using Abstractions;
    using Autodesk.AutoCAD.EditorInput;

    /// <inheritdoc />
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

        /// <inheritdoc />
        public void WriteAsNewLine(string message)
        {
            _editor.WriteMessage($"\n{message}");
        }
    }
}