namespace RxBim.Nuke.Revit.Generators.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Specifies Revit addin file.
    /// </summary>
    public class RevitAddIns
    {
        /// <summary>
        /// List of addins.
        /// </summary>
        public List<AddIn>? AddIn { get; set; }

        /// <summary>
        /// Revit addin manifest settings.
        /// </summary>
        public ManifestSettings? ManifestSettings { get; set; }
    }
}