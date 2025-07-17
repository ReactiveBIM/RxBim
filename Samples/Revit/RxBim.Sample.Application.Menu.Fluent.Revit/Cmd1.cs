namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using Command.Revit;
    using CSharpFunctionalExtensions;
    using PikTools.Ui.Abstractions;
    using RxBim.Command.Revit;
    using Shared;
    using Result = CSharpFunctionalExtensions.Result;

    /// <inheritdoc />
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [RxBimCommand(
        ToolTip = "Tooltip: I'm run command #1. Push me!", 
        Text = "Command\n#1", 
        Description = "Description: This is command #1", 
        LargeImage = @"img\num1_32.png",
        Image = @"img\num1_16.png",
        HelpUrl = "https://github.com/ReactiveBIM/RxBim")]
    public class Cmd1 : RxBimCommand
    {
        /*/// <inheritdoc />
        protected override bool RunInSeparatedContext => false;*/

        /// <summary>
        /// cmd.
        /// </summary>
        /// <param name="notificationService">123</param>
        public PluginResult ExecuteCommand(INotificationService notificationService)
        {
            const string appName = "Revit";
            var res = Result.Success()
                .Bind(() => Result.Success(appName))
                .Map(_ => appName);

            notificationService.ShowMessage(nameof(Cmd1), $"{res.Value} Command executed");
            return PluginResult.Succeeded;
        }
    }
}