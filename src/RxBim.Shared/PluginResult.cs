namespace RxBim.Shared
{
    using System.Collections.Generic;

    /// <summary>
    /// Результат выполнения команды
    /// </summary>
    public class PluginResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message">message</param>
        public PluginResult(string message)
        {
            Message = message;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="elementIdIds">element ids</param>
        public PluginResult(string message, List<int> elementIdIds)
        {
            Message = message;
            ElementIds = elementIdIds;
        }

        private PluginResult(Result result)
        {
            Result = result;
        }

        /// <summary>
        /// Succeced
        /// </summary>
        public static PluginResult Succeeded => new PluginResult(Result.Succeeded);

        /// <summary>
        /// Failed
        /// </summary>
        public static PluginResult Failed => new PluginResult(Result.Failed);

        /// <summary>
        /// Cancelled
        /// </summary>
        public static PluginResult Cancelled => new PluginResult(Result.Cancelled);

        /// <summary>
        /// adf
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// adf
        /// </summary>
        public List<int> ElementIds { get; set; } = new List<int>();

        /// <summary>
        /// sa
        /// </summary>
        public Result Result { get; set; }
    }
}