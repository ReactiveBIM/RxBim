﻿namespace RxBim.Shared
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
        /// <param name="elementIds">An element ids.</param>
        public PluginResult(string message, List<long> elementIds)
        {
            Message = message;
            ElementIds = elementIds;
        }

        private PluginResult(Result result)
        {
            Result = result;
        }

        /// <summary>
        /// Plugin execution succeeded.
        /// </summary>
        public static PluginResult Succeeded => new(Result.Succeeded);

        /// <summary>
        /// Plugin execution failed.
        /// </summary>
        public static PluginResult Failed => new(Result.Failed);

        /// <summary>
        /// Plugin execution cancelled.
        /// </summary>
        public static PluginResult Cancelled => new(Result.Cancelled);

        /// <summary>
        /// A message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Element ids.
        /// </summary>
        public List<long> ElementIds { get; set; } = new();

        /// <summary>
        /// The result.
        /// </summary>
        public Result Result { get; set; }
    }
}