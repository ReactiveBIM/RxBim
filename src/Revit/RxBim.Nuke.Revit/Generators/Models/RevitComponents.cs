namespace RxBim.Nuke.Revit.Generators.Models
{
    using System.Xml.Linq;
    using Helpers;
    using Nuke.Models;

    /// <inheritdoc />
    public class RevitComponents : Components
    {
        /// <inheritdoc />
        protected override XElement GetComponentEntry()
        {
            return new XElement(
                "ComponentEntry",
                new XAttribute(nameof(ModuleName), ModuleName.Ensure()));
        }
    }
}