namespace RxBim.Nuke.Models
{
    using System.Xml.Linq;
    using Helpers;

    /// <summary>
    /// Specifies components of a package manifest.
    /// </summary>
    public abstract class Components
    {
        /// <summary>
        /// Description.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Operation system.
        /// </summary>
        public string? OS { get; init; }

        /// <summary>
        /// Platform.
        /// </summary>
        public string? Platform { get; init; }

        /// <summary>
        /// Minimum platform version.
        /// </summary>
        public string? SeriesMin { get; init; }

        /// <summary>
        /// Maximum platform version.
        /// </summary>
        public string? SeriesMax { get; init; }

        /// <summary>
        /// Module name.
        /// </summary>
        public string? ModuleName { get; init; }

        /// <summary>
        /// Maps <see cref="Components"/> to <see cref="XElement"/>.
        /// </summary>
        public XElement ToXElement()
        {
            return new XElement(
                nameof(Components),
                new XAttribute(nameof(Description), Description.Ensure()),
                GetRuntimeRequirements(),
                GetComponentEntry());
        }

        /// <summary>
        /// Gets component entry.
        /// </summary>
        protected abstract XElement GetComponentEntry();

        private XElement GetRuntimeRequirements()
        {
            var element = new XElement(
                "RuntimeRequirements",
                new XAttribute(nameof(OS), OS.Ensure()),
                new XAttribute(nameof(Platform), Platform.Ensure()));

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