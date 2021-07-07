namespace PikTools.Command.Autocad.Example
{
    using Api;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;
    using Views;

    /// <summary>
    /// Команда для примера использования диалоговых окон
    /// </summary>
    public class Cmd2Dialogs : PikToolsCommand
    {
        /// <summary>
        /// Команда 2
        /// </summary>
        /// <param name="mainWindow">main window</param>
        public PluginResult ExecuteCommand(MainWindow mainWindow)
        {
            Application.ShowModalWindow(mainWindow);
            return PluginResult.Succeeded;
        }
    }
}