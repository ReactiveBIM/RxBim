namespace RxBim.Nuke.Generators.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// ApplicationPackage
    /// </summary>
    public class ApplicationPackage
    {
        /// <summary>
        /// Версия схемы
        /// </summary>
        public string SchemaVersion { get; set; } = "1.0";

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Версия приложения
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Совместимая версия
        /// </summary>
        public string FriendlyVersion { get; set; }

        /// <summary>
        /// Тип продукта
        /// </summary>
        public string ProductType { get; set; } = "Application";

        /// <summary>
        /// Код продукта
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Код обновления
        /// </summary>
        public string UpgradeCode { get; set; }

        /// <summary>
        /// Компоненты
        /// </summary>
        public List<Components> Components { get; set; }

        /// <summary>
        /// Маппит <see cref="ApplicationPackage"/>  в <see cref="XElement"/>
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