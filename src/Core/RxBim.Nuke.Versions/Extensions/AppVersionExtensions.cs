namespace RxBim.Nuke.Versions
{
    using System.Linq;
    using System.Text;
    using System.Xml;
    using global::Nuke.Common.ProjectModel;

    /// <summary>
    /// Extensions for <see cref="AppVersion"/>.
    /// </summary>
    internal static class AppVersionExtensions
    {
        /// <summary>
        /// Returns true if the <see cref="AppVersion"/> is applicable for the <see cref="Project"/>.
        /// Otherwise, returns false.
        /// </summary>
        /// <param name="appVersion"><see cref="AppVersion"/> object.</param>
        /// <param name="project"><see cref="Project"/> object.</param>
        public static bool IsApplicableFor(this AppVersion appVersion, Project project)
        {
            var appName = appVersion.AppType.ToString();
            return project.Directory.ToString().Contains(appName) || project.Name.Contains(appName);
        }

        /// <summary>
        /// Returns XML-string with project properties from <see cref="AppVersion"/> object.
        /// </summary>
        /// <param name="appVersion">Source <see cref="AppVersion"/> object.</param>
        public static string ToProjectProps(this AppVersion appVersion) =>
            appVersion
                .GenerateXmlDocument()
                .GetXmlString();

        private static XmlDocument GenerateXmlDocument(this AppVersion appVersion)
        {
            var doc = new XmlDocument();
            var element = appVersion.GenerateProjectElement(doc);
            doc.AppendChild(element);
            return doc;
        }

        private static XmlElement GenerateProjectElement(this AppVersion appVersion, XmlDocument doc)
        {
            var projectElement = doc.CreateElement("Project");
            projectElement.AddGroup(appVersion, doc, "PropertyGroup", SettingType.Property);
            projectElement.AddGroup(appVersion, doc, "ItemGroup", SettingType.Item);
            return projectElement;
        }

        private static void AddGroup(
            this XmlNode projectElement,
            AppVersion appVersion,
            XmlDocument doc,
            string groupName,
            SettingType type)
        {
            var group = doc.CreateElement(groupName);

            foreach (var item in appVersion.Settings.Where(x => x.Type == type))
                group.AppendChild(item.ToXmlElement(doc));

            projectElement.AppendChild(group);
        }

        private static XmlElement ToXmlElement(this ProjectSetting setting, XmlDocument doc)
        {
            var xmlElement = doc.CreateElement(setting.Name);

            foreach (var attribute in setting.Attributes)
                xmlElement.SetAttribute(attribute.Name, attribute.Value);

            if (!string.IsNullOrWhiteSpace(setting.Value))
                xmlElement.InnerText = setting.Value;

            return xmlElement;
        }

        private static string GetXmlString(this XmlDocument doc)
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
                doc.Save(writer);

            return sb.ToString();
        }
    }
}