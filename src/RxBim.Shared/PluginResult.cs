namespace RxBim.Shared
{
    using System.Collections.Generic;

    /// <summary>
    /// Specifies the result of a plugin.
    /// </summary>
    public class PluginResult
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="message">A message.</param>
        public PluginResult(string message)
        {
            Message = message;
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="message">A message.</param>
        /// <param name="elementIdIds">An element ids.</param>
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
        /// Plugin execution succeeded.
        /// </summary>
        public static PluginResult Succeeded => new PluginResult(Result.Succeeded);

        /// <summary>
        /// Plugin execution failed.
        /// </summary>
        public static PluginResult Failed => new PluginResult(Result.Failed);

        /// <summary>
        /// Plugin execution cancelled.
        /// </summary>
        public static PluginResult Cancelled => new PluginResult(Result.Cancelled);

        /// <summary>
        /// A message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Element ids.
        /// </summary>
        public List<int> ElementIds { get; set; } = new List<int>();

        /// <summary>
        /// The result.
        /// </summary>
        public Result Result { get; set; }
    }
}