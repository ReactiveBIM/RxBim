#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211

namespace RxBim.Nuke.Versions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Nuke.Common.Tooling;

    public partial class VersionNumber : Enumeration
    {
        public static implicit operator string(VersionNumber? number)
        {
            return number?.Value ?? string.Empty;
        }

        /// <summary>
        /// Returns all members of the enumeration.
        /// </summary>
        public static IEnumerable<VersionNumber> GetAll() =>
            typeof(VersionNumber).GetFields(BindingFlags.Public |
                                            BindingFlags.Static |
                                            BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<VersionNumber>();
    }
}