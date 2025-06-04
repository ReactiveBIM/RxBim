namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Autodesk.Revit.UI;
    using CSharpFunctionalExtensions;
    using Newtonsoft.Json;
    using RxBim.Application.Revit;
    using Shared;
    using Result = CSharpFunctionalExtensions.Result;

    /// <inheritdoc />
    public class App : RxBimApplication
    {
        /// <summary>
        /// Start application.
        /// </summary>
        public PluginResult Start()
        {
            var obj = new { Name = "123", Age = 15 };
            var json = JsonConvert.SerializeObject(obj);
            var res = Result.Success()
                .Bind(() =>
                {
                    var obj = new { Name = "123", Age = 15 };
                    var json = JsonConvert.SerializeObject(obj);
                    return Result.Success(json);
                })
                .Map(str => "123");
            TaskDialog.Show(GetType().Namespace, "App started");
            return PluginResult.Succeeded;
        }

        /// <summary>
        /// Shutdown application.
        /// </summary>
        public PluginResult Shutdown()
        {
            TaskDialog.Show(GetType().Namespace, "App finished");
            return PluginResult.Succeeded;
        }
    }
}