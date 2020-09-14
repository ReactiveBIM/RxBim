namespace PikTools.Nuke
{
    public class AssemblyType
    {
        public AssemblyType(string fullName, string baseTypeName)
        {
            FullName = fullName;
            BaseTypeName = baseTypeName;
        }

        public string FullName { get; }

        public string BaseTypeName { get; }

        public override string ToString()
        {
            return $"{FullName} : {BaseTypeName}";
        }
    }
}