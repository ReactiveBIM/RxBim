namespace RxBim.Nuke.Revit.Generators.Models
{
    using RxBim.Nuke.Models;

    /// <summary>
    /// Specifies an addin file data.
    /// </summary>
    public class AddIn
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="addInId">The addin identifier.</param>
        /// <param name="fullClassName">Full class name.</param>
        /// <param name="type">The plugin type.</param>
        public AddIn(string name, string assembly, string addInId, string fullClassName, PluginType type)
        {
            Name = name;
            Assembly = assembly;
            AddInId = addInId;
            FullClassName = fullClassName;
            Type = type;
        }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Assembly.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// AddInId.
        /// </summary>
        public string AddInId { get; set; }

        /// <summary>
        /// FullClassName.
        /// </summary>
        public string FullClassName { get; set; }

        /// <summary>
        /// Type.
        /// </summary>
        public PluginType Type { get; set; }

        /// <summary>
        /// Vendor id.
        /// </summary>
        public string VendorId { get; set; } = "PIK";

        /// <summary>
        /// Vendor description.
        /// </summary>
        public string VendorDescription { get; set; } = "PIK, http://pik.ru";
    }
}