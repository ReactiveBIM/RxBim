namespace RxBim.Sample.Command.Revit
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using Models;
    using RxBim.Command.Revit;
    using Shared;

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
            TaskDialog.Show(
                "RxBim.Sample.Command.Revit",
                $"Current environment variable = {settings.EnvironmentVariable}");
            
            return PluginResult.Succeeded;
        }
    }
}