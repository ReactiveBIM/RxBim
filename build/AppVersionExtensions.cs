using System.Text;
using System.Xml;

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

        propertyGroupElement.AppendChild(doc.GenerateElement(nameof(appVersion.TargetFramework), appVersion.TargetFramework));
        propertyGroupElement.AppendChild(doc.GenerateElement("AppVersion", appVersion.Version));

        return propertyGroupElement;
    }

    static XmlElement GenerateElement(this XmlDocument doc, string name, string value)
    {
        var targetFrameworkElement = doc.CreateElement(name);
        targetFrameworkElement.InnerText = value;
        return targetFrameworkElement;
    }

    static XmlElement GenerateItemGroup(AppVersion appVersion, XmlDocument doc)
    {
        var items = doc.CreateElement("ItemGroup");
        foreach (var reference in appVersion.PackageReferences)
        {
            var packageReferenceElement = doc.CreateElement("PackageReference");
            packageReferenceElement.SetAttribute("Include", reference.Name);
            packageReferenceElement.SetAttribute("Version", reference.Version);
            packageReferenceElement.SetAttribute("ExcludeAssets", "runtime");

            items.AppendChild(packageReferenceElement);
        }

        return items;
    }

    static string GetXmlString(this XmlDocument doc)
    {
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = true, IndentChars = "  ", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace
        };

        using (var writer = XmlWriter.Create(sb, settings))
        {
            doc.Save(writer);
        }

        return sb.ToString();
    }
}