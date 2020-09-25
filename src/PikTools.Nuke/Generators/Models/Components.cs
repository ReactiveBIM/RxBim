namespace PikTools.Nuke.Generators.Models
{
    /// <summary>
    /// Компонент
    /// </summary>
    public class Components
    {
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Операционая система
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
    }
}