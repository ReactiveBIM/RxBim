namespace RxBim.Sample.Command.Revit
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using CSharpFunctionalExtensions;
    using Models;
    using RxBim.Command.Revit;
    using Shared;
    using Result = CSharpFunctionalExtensions.Result;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    public class CheckEnvironmentCmd : RxBimCommand
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="settings"><see cref="PluginSettings"/></param>
        public PluginResult ExecuteCommand(PluginSettings settings)
        {
            const string appName = "Revit";
            var res = Result.Success()
                .Bind(Result.Success)
                .Map(_ => appName, appName);

            TaskDialog.Show(
                "RxBim.Sample.Command.Revit",
                $"Current {res.Value} environment variable = {settings.EnvironmentVariable}");

            return PluginResult.Succeeded;
        }
    }
}