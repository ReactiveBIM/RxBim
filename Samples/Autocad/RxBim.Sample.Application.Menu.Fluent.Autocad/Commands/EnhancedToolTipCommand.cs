namespace RxBim.Sample.Application.Menu.Fluent.Autocad.Commands;

using Autodesk.AutoCAD.ApplicationServices.Core;
using RxBim.Command.Autocad;
using Shared;
using Shared.Autocad;

/// <summary>
/// Confirms that enhanced tooltips do not interfere with command execution.
/// </summary>
[RxBimCommandClass("EnhancedToolTipExample")]
public class EnhancedToolTipCommand : RxBimCommand
{
    /// <summary>
    /// Displays confirmation without changing the drawing.
    /// </summary>
    public PluginResult ExecuteCommand()
    {
        Application.ShowAlertDialog("The enhanced tooltip command was executed.");
        return PluginResult.Succeeded;
    }
}