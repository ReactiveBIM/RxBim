namespace PikTools.CommandExample
{
    using Command.Api;
    using Shared;
    using Views;

    /// <summary>
    /// asdfaasd
    /// </summary>
    public class Cmd2 : PikToolsCommand
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