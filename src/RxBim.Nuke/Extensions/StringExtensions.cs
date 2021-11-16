namespace RxBim.Nuke.Extensions
{
    using System;
    using Models;
    using static Constants;

    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Type name of <see cref="PluginType"/>
        /// </summary>
        /// <param name="type">Type name</param>
        public static PluginType ToPluginType(this string type)
        {
            return type switch
            {
                RxBimCommand => PluginType.Command,
                RxBimApplication => PluginType.Application,
                _ => throw new NotSupportedException()
            };
        }
    }
}