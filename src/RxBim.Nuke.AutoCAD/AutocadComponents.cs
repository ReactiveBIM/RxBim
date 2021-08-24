namespace RxBim.Nuke.AutoCAD
{
    using System.Xml.Linq;
    using Generators.Models;

    /// <inheritdoc />
    public class AutocadComponents : Components
    {
        /// <inheritdoc />
        protected override XElement GetComponentEntry()
        {
            return new XElement(
                "ComponentEntry",
                new XAttribute(nameof(ModuleName), ModuleName),
                new XAttribute("AppType", ".Net"),
                new XAttribute("LoadOnAutoCADStartup", "True"));
        }
    }
}