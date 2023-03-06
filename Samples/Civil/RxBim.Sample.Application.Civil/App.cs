using Autodesk.AutoCAD.Runtime;
using RxBim.Sample.Application.Civil;

// You must explicitly specify the application class using this attribute.
[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Sample.Application.Civil
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using JetBrains.Annotations;
    using RxBim.Application.Civil;
    using Shared;

    /// <inheritdoc />
    [PublicAPI]
    public class App : RxBimApplication
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            CivilNotSupported += (_, args) =>
            {
                args.ShowMessage = false;
                Application.ShowAlertDialog("Is not Civil 3D!");
            };
        }

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