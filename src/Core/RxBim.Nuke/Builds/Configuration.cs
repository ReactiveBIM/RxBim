#pragma warning disable SA1401
namespace RxBim.Nuke.Builds
{
    extern alias NukeCommon;
    using System.ComponentModel;
    using NukeCommon::Nuke.Common.Tooling;

    /// <summary>
    /// Build configuration.
    /// </summary>
    [TypeConverter(typeof(TypeConverter<Configuration>))]
    public sealed class Configuration : Enumeration
    {
        /// <summary>
        /// Debug.
        /// </summary>
        public static Configuration Debug = new() { Value = nameof(Debug) };

        /// <summary>
        /// Release.
        /// </summary>
        public static Configuration Release = new() { Value = nameof(Release) };

        /// <summary>
        /// Casts an instance of type <see cref="Configuration"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        public static implicit operator string(Configuration configuration)
        {
            return configuration.Value;
        }
    }
}