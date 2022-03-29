namespace RxBim.Nuke.Models
{
    /// <summary>
    /// Type from assembly
    /// </summary>
    public class AssemblyType
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="fullName">Full name</param>
        /// <param name="baseTypeName">Base type name</param>
        public AssemblyType(string assemblyName, string fullName, string baseTypeName)
        {
            AssemblyName = assemblyName;
            FullName = fullName;
            BaseTypeName = baseTypeName;
        }

        /// <summary>
        /// Assembly name
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Base type name
        /// </summary>
        public string BaseTypeName { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{FullName} : {BaseTypeName}";
        }
    }
}