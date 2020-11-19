namespace PikTools.Application.Menu.Fluent.Example
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Command.Api;
    using Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class MyCmd : PikToolsCommand
    {
        /// <summary>
        /// cmd
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            TaskDialog.Show(nameof(MyCmd), "Command executed");
            return PluginResult.Succeeded;
        }

        /// <inheritdoc/>
        public override bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            return true;
        }
    }
}