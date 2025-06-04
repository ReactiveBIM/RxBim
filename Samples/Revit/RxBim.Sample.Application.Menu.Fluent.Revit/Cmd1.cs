namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using Command.Revit;
    using CSharpFunctionalExtensions;
    using Newtonsoft.Json;
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
        /// <summary>
        /// cmd.
        /// </summary>
        public PluginResult ExecuteCommand()
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
            
            TaskDialog.Show(nameof(Cmd1), "Command executed");
            return PluginResult.Succeeded;
        }
    }
}