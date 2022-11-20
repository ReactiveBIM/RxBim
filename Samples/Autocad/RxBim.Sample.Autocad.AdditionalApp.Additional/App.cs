using Autodesk.AutoCAD.Runtime;
using RxBim.Sample.Autocad.AdditionalApp;

// You must explicitly specify the application class using this attribute.
[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Sample.Autocad.AdditionalApp
{
    using System.Reflection;
    using Application.Autocad;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;

    /// <inheritdoc />
    public class App : RxBimApplication
    {
        private readonly string _name = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// This method is run when AutoCAD enters the first idle state after the application is loaded.
        /// </summary>
        public PluginResult Start()
        {
            Application.ShowAlertDialog($"{_name} started!");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// This method is run at the start of the AutoCAD shutdown process.
        /// </summary>
        public PluginResult Shutdown()
        {
            Application.ShowAlertDialog($"{_name} finished!");
            return PluginResult.Succeeded;
        }
    }
}