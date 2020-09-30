namespace PikTools.Nuke.Generators.Models
{
    using System.Collections.Generic;

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
        /// Версия прилодения
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
    }
}