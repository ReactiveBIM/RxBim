namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Autodesk.Revit.UI;
    using CSharpFunctionalExtensions;
    using PikTools.Ui.Abstractions;
    using RxBim.Application.Revit;
    using Shared;
    using Result = CSharpFunctionalExtensions.Result;

    /// <inheritdoc />
    public class App : RxBimApplication
    {
        /*/// <inheritdoc />
        protected override bool RunInSeparatedContext => false;*/

        /// <summary>
        /// Start application.
        /// </summary>
        /// <param name="notificationService">s</param>
        public PluginResult Start(INotificationService notificationService)
        {
            const string appName = "Revit";
            var res = Result.Success()
                .Bind(() => Result.Success(appName))
                .Map(_ => appName);
            TaskDialog.Show(GetType().Namespace, $"{res.Value} App started");
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