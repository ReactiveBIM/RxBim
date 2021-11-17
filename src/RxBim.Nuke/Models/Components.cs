namespace RxBim.Nuke.Models
{
    using System.Xml.Linq;

    /// <summary>
    /// Components
    /// </summary>
    public abstract class Components
    {
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Operation system
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Platform
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Min platform version
        /// </summary>
        public string SeriesMin { get; set; }

        /// <summary>
        /// Max platform version
        /// </summary>
        public string SeriesMax { get; set; }

        /// <summary>
        /// Module name
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Maps <see cref="Components"/> to <see cref="XElement"/>
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
        /// Gets component entry
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