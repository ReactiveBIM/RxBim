#pragma warning disable CS1591, SA1401, SA1600, SA1601, CA2211

namespace RxBim.Nuke.Versions
{
    using global::Nuke.Common.Tooling;

    public partial class VersionNumber : Enumeration
    {
        public static implicit operator string(VersionNumber number)
        {
            return number.Value;
        }
    }
}