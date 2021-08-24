namespace RxBim.Nuke.Generators.Models
{
    using System.Xml.Linq;

    /// <summary>
    /// Компонент
    /// </summary>
    public abstract class Components
    {
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Операционная система
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Платформа
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Минимальная версия платформы
        /// </summary>
        public string SeriesMin { get; set; }

        /// <summary>
        /// Максимальная версия платформы
        /// </summary>
        public string SeriesMax { get; set; }

        /// <summary>
        /// Имя модуля
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Маппит <see cref="Components"/>  в <see cref="XElement"/>
        /// </summary>
        public XElement ToXElement()
        {
            return new XElement(
                nameof(Components),
                new XAttribute(nameof(Description), Description),
                GetRuntimeRequirements(),
                GetComponentEntry());
        }

        /// <summary>
        /// Получение ComponentEntry
        /// </summary>
        protected abstract XElement GetComponentEntry();

        private XElement GetRuntimeRequirements()
        {
            var element = new XElement(
                "RuntimeRequirements",
                new XAttribute(nameof(OS), OS),
                new XAttribute(nameof(Platform), Platform));

            if (!string.IsNullOrWhiteSpace(SeriesMin))
            {
                element.Add(new XAttribute(nameof(SeriesMin), SeriesMin));
            }

            if (!string.IsNullOrWhiteSpace(SeriesMax))
            {
                element.Add(new XAttribute(nameof(SeriesMax), SeriesMax));
            }

            return element;
        }
    }
}