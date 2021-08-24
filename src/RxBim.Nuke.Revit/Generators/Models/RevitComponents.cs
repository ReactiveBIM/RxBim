namespace RxBim.Nuke.Revit.Generators.Models
{
    using System.Xml.Linq;
    using RxBim.Nuke.Generators.Models;

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