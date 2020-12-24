namespace PikTools.Nuke
{
    /// <summary>
    /// Тип извлеченный из сборки
    /// </summary>
    public class AssemblyType
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="assemblyName">Имя сборки</param>
        /// <param name="fullName">полное имя</param>
        /// <param name="baseTypeName">базовый тип</param>
        public AssemblyType(
            string assemblyName, string fullName, string baseTypeName)
        {
            AssemblyName = assemblyName;
            FullName = fullName;
            BaseTypeName = baseTypeName;
        }

        /// <summary>
        /// Имя сборки
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Полное имя
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Базовый тип
        /// </summary>
        public string BaseTypeName { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{FullName} : {BaseTypeName}";
        }
    }
}