namespace RxBim.Sample.Application.Menu.Config.Revit
{
    using Autodesk.Revit.UI;
    using RxBim.Application.Revit;
    using Shared;

    /// <summary>
    /// External Revit Application.
    /// </summary>
    public class App : RxBimApplication
    {
        /// <summary>
        /// Starts the application.
        /// </summary>
        public PluginResult Start()
        {
            TaskDialog.Show(GetType().Namespace, "App started");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// Shutdowns the application.
        /// </summary>
        public PluginResult Shutdown()
        {
            TaskDialog.Show(GetType().Namespace, "App finished");
            return PluginResult.Succeeded;
        }
    }
}