namespace PikTools.Command.Autocad.Example
{
    using Api;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Shared;

    /// <summary>
    /// Команда для примера логирования ошибок
    /// </summary>
    public class Cmd3WithExceptionForLogging : PikToolsCommand
    {
        /// <summary>
        /// Метод команды
        /// </summary>
        /// <param name="editor">Редактор документа</param>
        public PluginResult ExecuteCommand(Editor editor)
        {
            var objId = editor.GetEntity("\nВыберите объект: ").ObjectId;

            // Тут обязательно будет исключение, т.к. не запущена транзакция
            objId.GetObject(OpenMode.ForRead);

            return PluginResult.Succeeded;
        }
    }
}