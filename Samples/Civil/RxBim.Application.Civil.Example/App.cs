using Autodesk.AutoCAD.Runtime;
using RxBim.Application.Civil.Example;

// You must explicitly specify the application class using this attribute.
[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Application.Civil.Example
{
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
        public PluginResult Start()
        {
            Application.ShowAlertDialog("RxBimApplication for Civil started!");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// This method is run at the start of the AutoCAD shutdown process.
        /// </summary>
        public PluginResult Shutdown()
        {
            Application.ShowAlertDialog("RxBimApplication for Civil finished!");
            return PluginResult.Succeeded;
        }
    }
}