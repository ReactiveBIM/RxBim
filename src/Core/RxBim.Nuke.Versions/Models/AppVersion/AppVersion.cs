namespace RxBim.Nuke.Versions
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using global::Nuke.Common.Tooling;

    /// <summary>
    /// Settings for a particular version of the CAD application.
    /// </summary>
    [TypeConverter(typeof(TypeConverter<AppVersion>))]
    public partial class AppVersion : Enumeration
    {
        private AppVersion(string description, AppType type, params ProjectSetting[] settings)
        {
            Description = description;
            Type = type;
            Settings = settings;
        }

        /// <summary>Description of the version.</summary>
        public string Description { get; }

        /// <summary>Type of the CAD application.</summary>
        public AppType Type { get; }

        /// <summary>Settings collection.</summary>
        public ProjectSetting[] Settings { get; }

        /// <summary>
        /// Returns all members of the enumeration.
        /// </summary>
        public static IEnumerable<AppVersion> GetAll() =>
            typeof(AppVersion).GetFields(BindingFlags.Public |
                                         BindingFlags.Static |
                                         BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<AppVersion>();
    }
}