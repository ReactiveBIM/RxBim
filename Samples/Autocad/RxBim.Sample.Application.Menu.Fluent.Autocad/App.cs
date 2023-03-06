using Autodesk.AutoCAD.Runtime;
using RxBim.Sample.Application.Menu.Fluent.Autocad;

[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Sample.Application.Menu.Fluent.Autocad
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using RxBim.Application.Autocad;
    using Shared;

    /// <inheritdoc />
    public class App : RxBimApplication
    {
        /// <summary>
        /// Start application.
        /// </summary>
        public PluginResult Start()
        {
            Application.ShowAlertDialog($"{GetType().FullName} started!");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// Shutdown application.
        /// </summary>
        public PluginResult Shutdown()
        {
            Application.ShowAlertDialog($"{GetType().FullName} finished!");
            return PluginResult.Succeeded;
        }
    }
}