namespace RxBim.Nuke.Revit.Generators.Extensions
{
    using RxBim.Nuke.Extensions;
    using RxBim.Nuke.Models;

    /// <summary>
    /// Extensions for <see cref="AssemblyType"/> class.
    /// </summary>
    public static class AssemblyTypeExtensions
    {
        /// <summary>
        /// Generates <see cref="string"/> from the <see cref="AssemblyType"/>.
        /// </summary>
        /// <param name="type">The <see cref="AssemblyType"/>.</param>
        public static string ToPropertyName(this AssemblyType type)
        {
            return $"{type.BaseTypeName.ToPluginType()}__{type.FullName.ToPropertyName()}";
        }

        /// <summary>
        /// Maps <see cref="string"/> csproj property name via replacing all dots to underscore.
        /// </summary>
        /// <param name="str">The source string.</param>
        private static string ToPropertyName(this string str)
        {
            return str.Replace(".", "_");
        }
    }
}