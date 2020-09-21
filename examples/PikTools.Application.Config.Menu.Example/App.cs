namespace PikTools.Application.Config.Menu.Example
{
    using Api;
    using Autodesk.Revit.UI;
    using Shared;

    /// <summary>
    /// app
    /// </summary>
    public class App : PikToolsApplication
    {
        /// <summary>
        /// start
        /// </summary>
        public PluginResult Start()
        {
            TaskDialog.Show("PikTools.Application.Menu.Config.Example", "App started");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// shutdown
        /// </summary>
        public PluginResult ShutDown()
        {
            TaskDialog.Show("PikTools.Application.Menu.Config.Example", "App finished");
            return PluginResult.Succeeded;
        }
    }
}