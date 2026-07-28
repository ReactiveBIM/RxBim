namespace RxBim.Nuke.Revit.Generators.Models
{
    /// <summary>
    /// Specifies Revit addin manifest settings.
    /// </summary>
    public class ManifestSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether Revit's assembly load context should be used.
        /// </summary>
        public bool? UseRevitContext { get; set; }

        /// <summary>
        /// Gets or sets the custom assembly load context name.
        /// </summary>
        public string? ContextName { get; set; }
    }
}