namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using JetBrains.Annotations;

    /// <summary>
    /// Расширения для строк
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        /// <summary>
        /// Выводит текстовое сообщение в новой строке команд
        /// </summary>
        /// <param name="message">Сообщение</param>
        public static void WriteToCommandLineAsNewLine(this string message)
        {
            var editor = Application.DocumentManager.MdiActiveDocument?.Editor;
            editor?.WriteMessage($"\n{message}");
        }
    }
}