namespace RxBim.Application.Ribbon.Abstractions
{
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using ComboBox = Application.Ribbon.ComboBox;

    /// <summary>
    /// Service for <see cref="IRibbonPanelItem"/>.
    /// </summary>
    public interface IRibbonPanelItemService
    {
        /// <summary>
        /// Creates and returns a command button.
        /// </summary>
        /// <param name="button">Command button configuration.</param>
        PushButtonData CreateCommandButtonData(CommandButton button);

        /// <summary>
        /// Creates and returns combobox.
        /// </summary>
        /// <param name="tabName">Tab name.</param>
        /// <param name="comboBox">Combobox configuration.</param>
        RibbonCombo CreateComboBox(string tabName, ComboBox comboBox);

        /// <summary>
        /// Checks button name. If name is not set, throws exception.
        /// </summary>
        /// <param name="buttonConfig">Button configuration.</param>
        void CheckButtonName(Button buttonConfig);

        /// <summary>
        /// Sets tooltip.
        /// </summary>
        /// <param name="buttonData">Button data.</param>
        /// <param name="tooltip">Tooltip content.</param>
        void SetTooltip(RibbonItemData buttonData, string? tooltip);

        /// <summary>
        /// Sets the general properties of the button.
        /// </summary>
        /// <param name="buttonData">Button data.</param>
        /// <param name="buttonConfig">Button configuration.</param>
        void SetButtonProperties(ButtonData buttonData, Button buttonConfig);

        /// <summary>
        /// Creates buttons for Pull-Down button.
        /// </summary>
        /// <param name="config">Pull-Down button config.</param>
        /// <param name="button">Revit Pull-Down button instance.</param>
        void CreateButtonsForPullDown(PullDownButton config, PulldownButton button);

        /// <summary>
        /// Sets <see cref="ComboBox"/> properties.
        /// </summary>
        /// <param name="config">Combobox config.</param>
        /// <param name="comboBox">Revit combobox instance.</param>
        /// <param name="tabName">Tab name.</param>
        /// <param name="panelName">Panel name.</param>
        void SetComboBoxProperties(
            ComboBox config,
            Autodesk.Revit.UI.ComboBox comboBox,
            string tabName,
            string panelName);
    }
}