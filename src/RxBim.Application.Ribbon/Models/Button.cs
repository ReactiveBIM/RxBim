namespace RxBim.Application.Ribbon.Models
{
    /// <summary>
    /// Configuration for a button
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Button name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Button label text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The name of the button command class type (as the return value of the nameof operator)
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// URI string for large button image
        /// </summary>
        public string LargeImage { get; set; }

        /// <summary>
        /// URI string for small button image
        /// </summary>
        public string SmallImage { get; set; }

        /// <summary>
        /// Button description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Button tooltip
        /// </summary>
        public string ToolTip { get; set; }
    }
}