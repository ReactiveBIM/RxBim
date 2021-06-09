namespace PikTools.Nuke.Revit.Generators.Models
{
    using System.Xml.Linq;
    using Nuke.Generators.Models;

    /// <inheritdoc />
    public class RevitComponents : Components
    {
        /// <inheritdoc />
        protected override XElement GetComponentEntry()
        {
            return new XElement(
                "ComponentEntry",
                new XAttribute(nameof(ModuleName), ModuleName));
        }
    }
}