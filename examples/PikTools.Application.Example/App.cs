namespace PikTools.Application.Example
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
        /// <param name="service">service</param>
        public PluginResult Start(IService service)
        {
            service.Go();
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// shutdown
        /// </summary>
        public PluginResult ShutDown()
        {
            TaskDialog.Show("PikTools.Application.Example", "App finished");
            return PluginResult.Succeeded;
        }
    }
}