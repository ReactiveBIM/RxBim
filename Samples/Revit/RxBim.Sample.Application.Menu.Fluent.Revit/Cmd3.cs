namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using Command.Revit;
    using Shared;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [RxBimCommand(
        ToolTip = "Tooltip: I'm run command #3. Push me!",
        Text = "Command\n#3",
        Description = "Description: This is command #3",
        Image = @"img\num3_16.jpg",
        LargeImage = @"img\num3_32.jpg",
        SmallImageLight = @"img\num3_16_light.jpg",
        LargeImageLight = @"img\num3_32_light.jpg",
        HelpUrl = "https://www.autodesk.com/")]
    public class Cmd3 : RxBimCommand
    {
#if NETCOREAPP
        /// <inheritdoc />
        protected override bool RunInSeparatedContext => true;

        /// <inheritdoc />
        protected override bool ReuseSeparatedContext => true;
#endif

        /// <summary>
        /// cmd.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            TaskDialog.Show(nameof(Cmd3), "Command executed");
            return PluginResult.Succeeded;
        }
    }
}