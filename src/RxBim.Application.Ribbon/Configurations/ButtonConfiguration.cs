namespace RxBim.Application.Ribbon.Configurations
{
    /// <summary>
    /// Конфигурация кнопки
    /// </summary>
    public class ButtonConfiguration
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Тип команды
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// Большая картинка
        /// </summary>
        public string LargeImage { get; set; }

        /// <summary>
        /// Маленькая картинка
        /// </summary>
        public string SmallImage { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Всплывающее описание
        /// </summary>
        public string ToolTip { get; set; }
    }
}