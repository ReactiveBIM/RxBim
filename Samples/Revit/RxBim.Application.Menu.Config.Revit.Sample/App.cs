namespace RxBim.Application.Menu.Config.Revit.Sample
{
    using Application.Revit;
    using Autodesk.Revit.UI;
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