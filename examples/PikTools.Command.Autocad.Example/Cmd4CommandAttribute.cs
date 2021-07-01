namespace PikTools.Command.Autocad.Example
{
    using Api;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Runtime;
    using Shared;
    using Views;

    /// <summary>
    /// Команда для примера использования атрибута командного класса.
    /// Команда будет сгенерирована с именем Cmd4CommandAttributeExample, а не с именем класса.
    /// Команда не будет запускаться в пространстве листа и в редакторе блоков.
    /// </summary>
    [PikToolsCommandClass("Cmd4CommandAttributeExample",
        CommandFlags.Modal | CommandFlags.NoPaperSpace | CommandFlags.NoBlockEditor)]
    public class Cmd4CommandAttribute : PikToolsCommand
    {
        /// <summary>
        /// Команда
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello world!");
            return PluginResult.Succeeded;
        }
    }
}