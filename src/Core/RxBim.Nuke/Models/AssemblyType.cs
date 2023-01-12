namespace RxBim.Nuke.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Specifies the type from an assembly.
    /// </summary>
    public class AssemblyType
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AssemblyType"/> class.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="fullName">The assembly type full name.</param>
        /// <param name="baseTypeNames">The assembly type base types names.</param>
        public AssemblyType(string assemblyName, string fullName, IEnumerable<string> baseTypeNames)
        {
            AssemblyName = assemblyName;
            FullName = fullName;
            BaseTypeNames = baseTypeNames.ToList();
        }

        /// <summary>
        /// The assembly name.
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// The assembly full name.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// The assembly base type name.
        /// </summary>
        public List<string> BaseTypeNames { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{FullName} : {BaseTypeNames.FirstOrDefault()}";
        }
    }
}