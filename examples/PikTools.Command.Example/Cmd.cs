namespace PikTools.CommandExample
{
    using Autodesk.Revit.Attributes;
    using Command.Api;
    using PikTools.CommandExample.Views;
    using Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd : PikToolsCommand
    {
        /// <summary>
        /// cmd
        /// </summary>
        /// <param name="mainWindow">main window</param>
        public PluginResult ExecuteCommand(MainWindow mainWindow)
        {
            mainWindow.Show();
            return PluginResult.Succeeded;
        }
    }
}