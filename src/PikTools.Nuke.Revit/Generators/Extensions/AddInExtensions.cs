namespace PikTools.Nuke.Revit.Generators.Extensions
{
    using System.Linq;
    using System.Xml.Linq;
    using JetBrains.Annotations;
    using Models;

    /// <summary>
    /// Расширение для <see cref="AddIn"/>
    /// </summary>
    [PublicAPI]
    internal static class AddInExtensions
    {
        /// <summary>
        /// Преобразовывает <see cref="AddIn"/> в <see cref="XElement"/>
        /// </summary>
        /// <param name="addIn">addin</param>
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
        /// Преобразовывает <see cref="RevitAddIns"/> в <see cref="ToXDocument"/>
        /// </summary>
        /// <param name="revitAddIns">RevitAddIns</param>
        public static XDocument ToXDocument(this RevitAddIns revitAddIns)
        {
            return new XDocument(new XElement(nameof(RevitAddIns),
                revitAddIns.AddIn.Select(x => x.ToXElement())));
        }
    }
}