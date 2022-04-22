using System.Linq;
using System.Text;
using System.Xml;

namespace Versions;

public static class AppVersionExtensions
{
    public static string ToProjectProps(this AppVersion appVersion) => appVersion
        .GenerateXmlDocument()
        .GetXmlString();

    static XmlDocument GenerateXmlDocument(this AppVersion appVersion)
    {
        var doc = new XmlDocument();
        doc.AppendChild(GenerateProjectElement(appVersion, doc));

        return doc;
    }

    static XmlElement GenerateProjectElement(AppVersion appVersion, XmlDocument doc)
    {
        var projectElement = doc.CreateElement("Project");
        projectElement.AppendChild(GeneratePropertyGroup(appVersion, doc));
        projectElement.AppendChild(GenerateItemGroup(appVersion, doc));

        return projectElement;
    }

    static XmlElement GeneratePropertyGroup(AppVersion appVersion, XmlDocument doc)
    {
        var propertyGroupElement = doc.CreateElement("PropertyGroup");

        foreach (var prop in appVersion.Properties.Where(x => !x.IsItem))
        {
            propertyGroupElement.AppendChild(prop.ToXmlElement(doc));
        }

        return propertyGroupElement;
    }

    static XmlElement GenerateItemGroup(AppVersion appVersion, XmlDocument doc)
    {
        var items = doc.CreateElement("ItemGroup");
        foreach (var item in appVersion.Properties.Where(x => x.IsItem))
        {
            items.AppendChild(item.ToXmlElement(doc));
        }

        return items;
    }

    static XmlElement ToXmlElement(this ProjectItem item, XmlDocument doc)
    {
        var xmlElement = doc.CreateElement(item.Name);
        if (item.ItemAttributes != null)
        {
            foreach (var attribute in item.ItemAttributes)
            {
                xmlElement.SetAttribute(attribute.Name, attribute.Value);
            }
        }

        if (!string.IsNullOrWhiteSpace(item.Value))
        {
            xmlElement.InnerText = item.Value;
        }

        return xmlElement;
    }

    static string GetXmlString(this XmlDocument doc)
    {
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = true, IndentChars = "    ", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace
        };

        using (var writer = XmlWriter.Create(sb, settings))
        {
            doc.Save(writer);
        }

        return sb.ToString();
    }
}