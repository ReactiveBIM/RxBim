using Autodesk.AutoCAD.Runtime;
using RxBim.Application.Autocad.Example;

[assembly: ExtensionApplication(typeof(App))]

namespace RxBim.Application.Autocad.Example
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;

    /// <inheritdoc />
    public class App : RxBimApplication
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
        public PluginResult Shutdown()
        {
            Application.ShowAlertDialog("RxBimApplication example app finished!");
            return PluginResult.Succeeded;
        }
    }
}