namespace PikTools.Command.Autocad.Example
{
    using Api;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Shared;

    /// <summary>
    /// Команда для примера использования текущего документа, редактора и базы данных текущего документа
    /// </summary>
    public class Cmd1AcadObjectsInjection : PikToolsCommand
    {
        /// <summary>
        /// Метод команды
        /// </summary>
        /// <param name="document">Документ</param>
        /// <param name="editor">Редактор документа</param>
        /// <param name="database">База данных документа</param>
        public PluginResult ExecuteCommand(Document document, Editor editor, Database database)
        {
            editor.WriteMessage("\nПуть к текущему документу:");
            editor.WriteMessage("\n{0}", document.Name);
            editor.WriteMessage("\nВерсия базы чертежа:");
            editor.WriteMessage("\n{0}", database.OriginalFileVersion.ToString());

            return PluginResult.Succeeded;
        }
    }
}