namespace PikTools.Shared
{
    using System.Collections.Generic;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Результат выполнения команды
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message">message</param>
        public CommandResult(string message)
        {
            Message = message;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="elements">elements</param>
        public CommandResult(string message, List<Element> elements)
        {
            Message = message;
            Elements = elements;
        }

        private CommandResult(Result result)
        {
            Result = result;
        }

        /// <summary>
        /// Succeced
        /// </summary>
        public static CommandResult Succeeded => new CommandResult(Result.Succeeded);

        /// <summary>
        /// Failed
        /// </summary>
        public static CommandResult Failed => new CommandResult(Result.Failed);

        /// <summary>
        /// Cancelled
        /// </summary>
        public static CommandResult Cancelled => new CommandResult(Result.Cancelled);

        /// <summary>
        /// adf
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// adf
        /// </summary>
        public List<Element> Elements { get; set; } = new List<Element>();

        /// <summary>
        /// sa
        /// </summary>
        public Result Result { get; set; }
    }
}