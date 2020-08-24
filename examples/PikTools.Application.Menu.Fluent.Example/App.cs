namespace PikTools.Application.Menu.Fluent.Example
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
            TaskDialog.Show("PikTools.Application.Menu.Fluent.Example", "App started");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// shutdown
        /// </summary>
        public PluginResult ShutDown()
        {
            TaskDialog.Show("PikTools.Application.Menu.Fluent.Example", "App finished");
            return PluginResult.Succeeded;
        }
    }
}