namespace RxBim.Nuke.Revit.Generators.Models
{
    using RxBim.Nuke.Extensions;
    using RxBim.Nuke.Generators.Models;
    using RxBim.Nuke.Models;

    /// <summary>
    /// Addin
    /// </summary>
    public class AddIn
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="assembly">assembly</param>
        /// <param name="addInId">addinid</param>
        /// <param name="fullClassName">fullclassname</param>
        /// <param name="type">type</param>
        public AddIn(string name, string assembly, string addInId, string fullClassName, PluginType type)
        {
            Name = name;
            Assembly = assembly;
            AddInId = addInId;
            FullClassName = fullClassName;
            Type = type;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assembly
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// AddInId
        /// </summary>
        public string AddInId { get; set; }

        /// <summary>
        /// FullClassName
        /// </summary>
        public string FullClassName { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public PluginType Type { get; set; }

        /// <summary>
        /// VendorId
        /// </summary>
        public string VendorId { get; set; } = "PIK";

        /// <summary>
        /// VendorDescription
        /// </summary>
        public string VendorDescription { get; set; } = "PIK, http://pik.ru";
    }
}