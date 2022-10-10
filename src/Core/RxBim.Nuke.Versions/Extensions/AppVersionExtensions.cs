namespace RxBim.Nuke.Versions
{
    using System.Linq;
    using System.Text;
    using System.Xml;

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
            projectElement.AddGroup(appVersion, doc, "PropertyGroup", SettingType.Property);
            projectElement.AddGroup(appVersion, doc, "ItemGroup", SettingType.Item);
            return projectElement;
        }

        static void AddGroup(
            this XmlNode projectElement,
            AppVersion appVersion,
            XmlDocument doc,
            string groupName,
            SettingType type)
        {
            var group = doc.CreateElement(groupName);

            foreach (var item in appVersion.Nodes.Where(x => x.Type == type))
                group.AppendChild(item.ToXmlElement(doc));

            projectElement.AppendChild(group);
        }

        static XmlElement ToXmlElement(this ProjectSettingBase settingBase, XmlDocument doc)
        {
            var xmlElement = doc.CreateElement(settingBase.Name);
            if (settingBase.Attributes != null)
            {
                foreach (var attribute in settingBase.Attributes)
                    xmlElement.SetAttribute(attribute.Name, attribute.Value);
            }

            if (!string.IsNullOrWhiteSpace(settingBase.Value))
                xmlElement.InnerText = settingBase.Value;

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
}