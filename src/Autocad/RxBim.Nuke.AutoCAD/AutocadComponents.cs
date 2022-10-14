namespace RxBim.Nuke.AutoCAD
{
    using System.Xml.Linq;
    using Helpers;
    using Models;

    /// <inheritdoc />
    public class AutocadComponents : Components
    {
        /// <inheritdoc />
        protected override XElement GetComponentEntry()
        {
            return new XElement(
                "ComponentEntry",
                new XAttribute(nameof(ModuleName), ModuleName.Ensure()),
                new XAttribute("AppType", ".Net"),
                new XAttribute("LoadOnAutoCADStartup", "True"));
        }
    }
}