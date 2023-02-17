namespace RxBim.Sample.Command.Autocad.Commands
{
    using Autodesk.AutoCAD.DatabaseServices;
    using JetBrains.Annotations;
    using RxBim.Command.Autocad;
    using Shared;

    /// <summary>
    /// When this command is executed, an exception occurs, information about which is written to the log file.
    /// </summary>
    [PublicAPI]
    public class GenerateExceptionForLoggingCmd : RxBimCommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="database"><see cref="Database"/> instance.</param>
        public PluginResult ExecuteCommand(Database database)
        {
            // There will definitely be an exception, because. transaction not started
            database.Clayer.GetObject(OpenMode.ForRead);

            return PluginResult.Failed;
        }
    }
}