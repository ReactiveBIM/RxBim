using Autodesk.AutoCAD.Runtime;
using RxBim.Application.Menu.Config.Autocad.Sample;

[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Application.Menu.Config.Autocad.Sample
{
    using Application.Autocad;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;

    /// <inheritdoc />
    public class App : RxBimApplication
    {
        /// <summary>
        /// Start application
        /// </summary>
        public PluginResult Start()
        {
            Application.ShowAlertDialog($"{GetType().FullName} started!");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// Shutdown application
        /// </summary>
        public PluginResult Shutdown()
        {
            Application.ShowAlertDialog($"{GetType().FullName} finished!");
            return PluginResult.Succeeded;
        }
    }
}