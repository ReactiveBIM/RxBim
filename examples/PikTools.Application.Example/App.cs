namespace PikTools.Application.Example
{
    using Api;
    using Autodesk.Revit.UI;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

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
            TaskDialog.Show("ddd", "Googby");
            return PluginResult.Succeeded;
        }
    }
}