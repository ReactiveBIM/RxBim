namespace PikTools.Nuke
{
    using System.Collections.Generic;

    public class RevitAddIns
    {
        public List<AddIn> AddIn { get; set; }
    }

    public class AddIn
    {
        public AddIn(string name, string assembly, string addInId, string fullClassName, PluginType type)
        {
            Name = name;
            Assembly = assembly;
            AddInId = addInId;
            FullClassName = fullClassName;
            Type = type;
        }

        public string Name { get; set; }

        public string Assembly { get; set; }

        public string AddInId { get; set; }

        public string FullClassName { get; set; }

        public PluginType Type { get; set; }

        public string VendorId { get; set; } = "PIK";

        public string VendorDescription { get; set; } = "PIK, http://pik.ru";
    }
}