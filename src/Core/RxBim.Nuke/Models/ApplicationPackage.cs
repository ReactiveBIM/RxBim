namespace RxBim.Nuke.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Helpers;

    /// <summary>
    /// Specifies an application package manifest.
    /// </summary>
    public class ApplicationPackage
    {
        /// <summary>
        /// Schema version.
        /// </summary>
        public string SchemaVersion { get; init; } = "1.0";

        /// <summary>
        /// Package name.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Description.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Application version.
        /// </summary>
        public string? AppVersion { get; init; }

        /// <summary>
        /// Friendly version.
        /// </summary>
        public string? FriendlyVersion { get; init; }

        /// <summary>
        /// Product type.
        /// </summary>
        public string ProductType { get; init; } = "Application";

        /// <summary>
        /// Product code.
        /// </summary>
        public string? ProductCode { get; init; }

        /// <summary>
        /// Upgrade code.
        /// </summary>
        public string? UpgradeCode { get; init; }

        /// <summary>
        /// Components.
        /// </summary>
        public List<Components>? Components { get; init; }

        /// <summary>
        /// Maps <see cref="ApplicationPackage"/> to <see cref="XElement"/>.
        /// </summary>
        public XElement ToXElement()
        {
            return new XElement(
                nameof(ApplicationPackage),
                new XAttribute(nameof(Description), Description.Ensure()),
                new XAttribute(nameof(SchemaVersion), SchemaVersion),
                new XAttribute(nameof(Name), Name.Ensure()),
                new XAttribute(nameof(AppVersion), AppVersion.Ensure()),
                new XAttribute(nameof(FriendlyVersion), FriendlyVersion.Ensure()),
                new XAttribute(nameof(ProductType), ProductType),
                new XAttribute(nameof(ProductCode), $"{{{ProductCode}}}"),
                new XAttribute(nameof(UpgradeCode), $"{{{UpgradeCode}}}"),
                Components.Ensure().Select(x => x.ToXElement()));
        }
    }
}