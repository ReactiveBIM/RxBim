namespace RxBim.Nuke.Revit.Generators.Extensions
{
    using System.Linq;
    using System.Xml.Linq;
    using JetBrains.Annotations;
    using Models;

    /// <summary>
    /// Extension methods for <see cref="AddIn"/>.
    /// </summary>
    [PublicAPI]
    internal static class AddInExtensions
    {
        /// <summary>
        /// Maps an <see cref="AddIn"/> to the <see cref="XElement"/>.
        /// </summary>
        /// <param name="addIn">An addin.</param>
        /// <returns>The <see cref="XElement"/> mapped from <paramref name="addIn"/>.</returns>
        public static XElement ToXElement(this AddIn addIn)
        {
            return new XElement(nameof(AddIn),
                new XAttribute(nameof(AddIn.Type), addIn.Type),
                new XElement(nameof(AddIn.Assembly), addIn.Assembly),
                new XElement(nameof(AddIn.Name), addIn.Name),
                new XElement(nameof(AddIn.VendorDescription), addIn.VendorDescription),
                new XElement(nameof(AddIn.VendorId), addIn.VendorId),
                new XElement(nameof(AddIn.AddInId), addIn.AddInId),
                new XElement(nameof(AddIn.FullClassName), addIn.FullClassName));
        }

        /// <summary>
        /// Maps a <see cref="RevitAddIns"/> to the <see cref="XDocument"/>.
        /// </summary>
        /// <param name="revitAddIns">RevitAddIns.</param>
        /// <returns>The <see cref="XDocument"/> mapped from <paramref name="revitAddIns"/>.</returns>
        public static XDocument ToXDocument(this RevitAddIns revitAddIns)
        {
            return new XDocument(new XElement(nameof(RevitAddIns),
                revitAddIns.AddIn.Select(x => x.ToXElement())));
        }
    }
}