namespace RxBim.Sample.Command.Autocad.Commands
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using JetBrains.Annotations;
    using RxBim.Command.Autocad;
    using Shared;

    /// <summary>
    /// Command for an example of using an AutoCAD document, editor, and database.
    /// </summary>
    [PublicAPI]
    public class ObjectsInjectionCmd : RxBimCommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="document"><see cref="Document"/> instance.</param>
        /// <param name="editor"><see cref="Editor"/> instance.</param>
        /// <param name="database"><see cref="Database"/> instance.</param>
        public PluginResult ExecuteCommand(Document document, Editor editor, Database database)
        {
            editor.WriteMessage("\nCurrent document path:");
            editor.WriteMessage("\n{0}", document.Name);
            editor.WriteMessage("\nDatabase version:");
            editor.WriteMessage("\n{0}", database.OriginalFileVersion.ToString());

            return PluginResult.Succeeded;
        }
    }
}