namespace PikTools.Nuke
{
    using System.Linq;
    using System.Xml.Linq;

    internal static class AddinExtensions
    {
        public static XElement ToXElement(this AddIn addIn)
        {
            return new XElement(nameof(AddIn), new XAttribute(nameof(AddIn.Type), addIn.Type),
                new XElement(nameof(AddIn.Assembly), addIn.Assembly),
                new XElement(nameof(AddIn.Name), addIn.Name),
                new XElement(nameof(AddIn.VendorDescription), addIn.VendorDescription),
                new XElement(nameof(AddIn.VendorId), addIn.VendorId),
                new XElement(nameof(AddIn.AddInId), addIn.AddInId),
                new XElement(nameof(AddIn.FullClassName), addIn.FullClassName)
            );
        }

        public static XDocument ToXDocument(this RevitAddIns revitAddIns)
        {
            return new XDocument(new XElement(nameof(RevitAddIns),
                revitAddIns.AddIn.Select(x => x.ToXElement())));
        }
    }
}