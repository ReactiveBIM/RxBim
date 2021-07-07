using Autodesk.AutoCAD.Runtime;
using PikTools.Application.Autocad.Example;

[assembly: ExtensionApplication(typeof(App))]

namespace PikTools.Application.Autocad.Example
{
    using Api;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Shared;

    /// <inheritdoc />
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
            Application.ShowAlertDialog(
                "PikToolsApplication example app finished!");
            return PluginResult.Succeeded;
        }
    }
}