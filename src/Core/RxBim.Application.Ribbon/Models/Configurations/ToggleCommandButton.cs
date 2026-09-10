namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Represents a toggle button that invokes a command.
    /// </summary>
    public class ToggleCommandButton : Button
    {
        /// <summary>
        /// A command to be invoked type name.
        /// </summary>
        public string? CommandType { get; set; }

        /// <summary>
        /// Initial checked state of the toggle button.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Indicates whether the toggle button supports three states.
        /// </summary>
        public bool IsThreeState { get; set; }

        /// <summary>
        /// Configuration marker that identifies the item as a toggle command button.
        /// </summary>
        public bool IsToggle { get; set; } = true;
    }
}