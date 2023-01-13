namespace RxBim.Command.Autocad.Example.Commands
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Runtime;
    using JetBrains.Annotations;
    using Shared;
    using Shared.Autocad;

    /// <summary>
    /// Command for an example of using the command class attribute.
    /// The command name will be generated from the <see cref="RxBimCommandAttribute"/> parameter, not from the class name.
    /// The command will not run in paper space and block editor.
    /// </summary>
    [RxBimCommandClass("RunCommandAttributeExample",
        CommandFlags.Modal | CommandFlags.NoPaperSpace | CommandFlags.NoBlockEditor)]
    [PublicAPI]
    public class CommandClassAttributeUsageCmd : RxBimCommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        public PluginResult ExecuteCommand()
        {
            Application.ShowAlertDialog("Hello world!");
            return PluginResult.Succeeded;
        }
    }
}