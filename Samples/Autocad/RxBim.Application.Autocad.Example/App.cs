using Autodesk.AutoCAD.Runtime;
using RxBim.Application.Autocad.Example;

// You must explicitly specify the application class using this attribute.
[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Application.Autocad.Example
{
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using JetBrains.Annotations;
    using Shared;

    /// <inheritdoc />
    [PublicAPI]
    public class App : RxBimApplication
    {
        /// <summary>
        /// This method is run when AutoCAD enters the first idle state after the application is loaded.
        /// </summary>
        /// <param name="service"><see cref="IInfoService"/> instance.</param>
        public PluginResult Start(IInfoService service)
        {
            Application.ShowAlertDialog("RxBimApplication example started!");
            service.ShowAutocadVersion();
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// This method is run at the start of the AutoCAD shutdown process.
        /// </summary>
        public PluginResult Shutdown()
        {
            Application.ShowAlertDialog("RxBimApplication example finished!");
            return PluginResult.Succeeded;
        }
    }
}