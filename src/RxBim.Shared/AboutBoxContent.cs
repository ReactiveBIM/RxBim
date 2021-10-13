namespace RxBim.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Содержимое окна о программе
    /// </summary>
    public class AboutBoxContent
    {
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
        /// Зоголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Версия продукта
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// Описание программы
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Версия сборки
        /// </summary>
        public Version BuildVersion { get; set; }

        /// <summary>
        /// Название компании
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Список ссылок
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