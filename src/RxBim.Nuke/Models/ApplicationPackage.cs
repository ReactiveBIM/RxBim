namespace RxBim.Nuke.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Specifies an application package manifest.
    /// </summary>
    public class ApplicationPackage
    {
        /// <summary>
        /// Schema version.
        /// </summary>
        public string SchemaVersion { get; set; } = "1.0";

        /// <summary>
        /// Package name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Application version.
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Friendly version.
        /// </summary>
        public string FriendlyVersion { get; set; }

        /// <summary>
        /// Product type.
        /// </summary>
        public string ProductType { get; set; } = "Application";

        /// <summary>
        /// Product code.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Upgrade code.
        /// </summary>
        public string UpgradeCode { get; set; }

        /// <summary>
        /// Components.
        /// </summary>
        public List<Components> Components { get; set; }

        /// <summary>
        /// Maps <see cref="ApplicationPackage"/> to <see cref="XElement"/>.
        /// </summary>
        public XElement ToXElement()
        {
            return new XElement(
                nameof(ApplicationPackage),
                new XAttribute(nameof(Description), Description),
                new XAttribute(nameof(SchemaVersion), SchemaVersion),
                new XAttribute(nameof(Name), Name),
                new XAttribute(nameof(AppVersion), AppVersion),
                new XAttribute(nameof(FriendlyVersion), FriendlyVersion),
                new XAttribute(nameof(ProductType), ProductType),
                new XAttribute(nameof(ProductCode), $"{{{ProductCode}}}"),
                new XAttribute(nameof(UpgradeCode), $"{{{UpgradeCode}}}"),
                Components.Select(x => x.ToXElement()));
        }
    }
}