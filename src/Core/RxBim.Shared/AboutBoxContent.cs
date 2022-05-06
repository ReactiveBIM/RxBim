#pragma warning disable
namespace RxBim.Application.Ribbon
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Содержимое окна о программе
    /// </summary>
    [Obsolete("Will be deleted in the next release!")]
    public class AboutBoxContent
    {
        private Version _version = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutBoxContent"/> class.
        /// </summary>
        public AboutBoxContent()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="title">Зоголовок</param>
        /// <param name="productVersion">Версия продукта</param>
        /// <param name="description">Описание программы</param>
        /// <param name="buildVersion">Версия сборки</param>
        /// <param name="companyName">Название компании</param>
        /// <param name="links">Список ссылок</param>
        public AboutBoxContent(
            string title,
            string productVersion,
            string description,
            Version buildVersion,
            string companyName,
            Dictionary<string, string> links)
        {
            Title = title;
            ProductVersion = productVersion;
            Description = description;
            BuildVersion = buildVersion;
            CompanyName = companyName;
            Links = links;
        }

        /// <summary>
        /// The title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The string representation of the product version.
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// The product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The product build number.
        /// </summary>
        public Version BuildVersion
        {
            get => _version;
            set => _version = value;
        }

        /// <summary>
        /// Версия сборки в виде строки
        /// </summary>
        public string BuildVersionString
        {
            get => _version.ToString();
            set => _version = new Version(value);
        }

        /// <summary>
        /// The company name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// The hyperlink list.
        /// </summary>
        public Dictionary<string, string> Links { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            var str = new StringBuilder();

            str.AppendLine($"{Title} {ProductVersion}");
            str.AppendLine(Description);
            str.AppendLine(BuildVersion.ToString());
            str.AppendLine(CompanyName);

            foreach (var link in Links)
                str.AppendLine($"{link.Key}: {link.Value}");

            return str.ToString();
        }
    }
}