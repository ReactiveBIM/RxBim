namespace RxBim.Sample.Command.Revit
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using Models;
    using Newtonsoft.Json;
    using RxBim.Command.Revit;
    using Shared;
    using Ser = System.Text.Json.JsonSerializer;

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

            var obj = new { Name = "123", Age = 15 };
            var json = JsonConvert.SerializeObject(obj);

            var j = Ser.Serialize(obj);

            return PluginResult.Succeeded;
        }
    }
}