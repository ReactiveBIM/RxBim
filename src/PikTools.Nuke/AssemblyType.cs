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
        /// <param name="fullName">полное имя</param>
        /// <param name="baseTypeName">базовый тип</param>
        public AssemblyType(string fullName, string baseTypeName)
        {
            FullName = fullName;
            BaseTypeName = baseTypeName;
        }

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