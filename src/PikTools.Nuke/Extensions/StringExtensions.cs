namespace PikTools.Nuke.Extensions
{
    using System;
    using Generators.Models;
    using Models;
    using static Constants;

    /// <summary>
    /// Расширения для строк
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Имя типа в <see cref="PluginType"/>
        /// </summary>
        /// <param name="type">имя типа</param>
        public static PluginType ToPluginType(this string type)
        {
            return type switch
            {
                PikToolsCommand => PluginType.Command,
                PikToolsApplication => PluginType.Application,
                _ => throw new NotSupportedException()
            };
        }
    }
}