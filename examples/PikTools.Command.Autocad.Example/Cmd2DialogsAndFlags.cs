namespace PikTools.Command.Autocad.Example
{
    using Api;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Runtime;
    using Shared;
    using Views;

    /// <summary>
    /// Команда для примера использования диалоговых окон и командных флагов
    /// </summary>
    [PikToolsCommandClass(CommandFlags.Modal | CommandFlags.NoPaperSpace | CommandFlags.NoBlockEditor)]
    public class Cmd2DialogsAndFlags : PikToolsCommand
    {
        /// <summary>
        /// Команда 1
        /// </summary>
        /// <param name="mainWindow">main window</param>
        public PluginResult ExecuteCommand(MainWindow mainWindow)
        {
            Application.ShowModalWindow(mainWindow);
            return PluginResult.Succeeded;
        }
    }
}