namespace RxBim.Nuke.Revit.Generators.Models
{
    /// <summary>
    /// Specifies Revit addin manifest settings.
    /// </summary>
    public class ManifestSettings
    {
        /// <summary>
        /// Gets or sets whether the add-in should use Revit's shared assembly load context.
        /// <see langword="false"/> enables a separate isolated add-in context;
        /// <see langword="true"/> disables isolation and uses Revit's context.
        /// When omitted, Revit 2026 and newer default to <see langword="true"/>.
        /// </summary>
        public bool? UseRevitContext { get; set; }

        /// <summary>
        /// Gets or sets the custom assembly load context name.
        /// </summary>
        public string? ContextName { get; set; }
    }
}