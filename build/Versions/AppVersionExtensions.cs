using System.Linq;
using System.Text;
using System.Xml;

namespace Versions;

public static class AppVersionExtensions
{
    public static string ToProjectProps(this AppVersion appVersion) =>
        appVersion
            .GenerateXmlDocument()
            .GetXmlString();

    static XmlDocument GenerateXmlDocument(this AppVersion appVersion)
    {
        var doc = new XmlDocument();
        var element = appVersion.GenerateProjectElement(doc);
        doc.AppendChild(element);
        return doc;
    }

    static XmlElement GenerateProjectElement(this AppVersion appVersion, XmlDocument doc)
    {
        var projectElement = doc.CreateElement("Project");
        projectElement.AddGroup(appVersion, doc, "PropertyGroup", ItemType.Property);
        projectElement.AddGroup(appVersion, doc, "ItemGroup", ItemType.Item);
        return projectElement;
    }

    static void AddGroup(
        this XmlNode projectElement,
        AppVersion appVersion,
        XmlDocument doc,
        string groupName,
        ItemType type)
    {
        var group = doc.CreateElement(groupName);

        foreach (var item in appVersion.Items.Where(x => x.Type == type))
            group.AppendChild(item.ToXmlElement(doc));

        projectElement.AppendChild(group);
    }

    static XmlElement ToXmlElement(this ProjectItem item, XmlDocument doc)
    {
        var xmlElement = doc.CreateElement(item.Name);
        if (item.Attributes != null)
        {
            foreach (var attribute in item.Attributes)
                xmlElement.SetAttribute(attribute.Name, attribute.Value);
        }

        if (!string.IsNullOrWhiteSpace(item.Value))
            xmlElement.InnerText = item.Value;

        return xmlElement;
    }

    static string GetXmlString(this XmlDocument doc)
    {
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "    ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace
        };

        using (var writer = XmlWriter.Create(sb, settings))
        {
            doc.Save(writer);
        }

        return sb.ToString();
    }
}