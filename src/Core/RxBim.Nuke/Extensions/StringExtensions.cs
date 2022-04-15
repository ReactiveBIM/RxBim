namespace RxBim.Nuke.Extensions
{
    using System;
    using Models;
    using static Constants;

    /// <summary>
    /// The <see cref="string"/> extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets type name of a <see cref="PluginType"/>.
        /// </summary>
        /// <param name="type">The plugin type name.</param>
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