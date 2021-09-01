namespace RxBim.Nuke.Builds
{
    using System.ComponentModel;
    using global::Nuke.Common.Tooling;

    /// <inheritdoc />
    [TypeConverter(typeof(TypeConverter<Configuration>))]
    public sealed class Configuration : Enumeration
    {
        /// <summary>
        /// Debug
        /// </summary>
        public static Configuration Debug = new Configuration { Value = nameof(Debug) };

        /// <summary>
        /// Release
        /// </summary>
        public static Configuration Release = new Configuration { Value = nameof(Release) };

        public static implicit operator string(Configuration configuration)
        {
            return configuration.Value;
        }
    }
}