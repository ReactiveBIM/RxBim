namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using Di;
    using Ribbon.Services;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Revit ribbon panel
    /// </summary>
    public class PanelBuilder : PanelBuilderBase<RibbonBuilder, StackedItemsBuilder, ButtonBuilder>
    {
        private readonly RibbonPanel _revitPanel;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbonBuilder"><see cref="RibbonBuilder"/></param>
        /// <param name="revitPanel"><see cref="RibbonPanel"/></param>
        /// <param name="container"><see cref="IContainer"/></param>
        /// <param name="tabBuilder">Parent tab</param>
        public PanelBuilder(RibbonBuilder ribbonBuilder, RibbonPanel revitPanel, IContainer container, ITabBuilder tabBuilder)
            : base(ribbonBuilder, container, tabBuilder)
        {
            _revitPanel = revitPanel;
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public override IPanelBuilder Button(string name, string text, Type externalCommandType, Action<IButtonBuilder> action = null)
        {
            var button = new ButtonBuilder(name, text, externalCommandType);
            action?.Invoke(button);
            var buttonData = (PushButtonData)button.GetButtonData();
            _revitPanel.AddItem(buttonData);
            return this;
        }

        /// <summary>
        /// Create pull down button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public override IPanelBuilder PullDownButton(string name, string text, Action<IPulldownButtonBuilder> action)
        {
            var button = new PulldownButtonBuilder(name, text);
            action?.Invoke(button);
            var buttonData = (PulldownButtonData)button.GetButtonData();
            var ribbonItem = _revitPanel.AddItem(buttonData) as Autodesk.Revit.UI.PulldownButton;
            button.BuildButtons(ribbonItem);
            return this;
        }

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        public override IPanelBuilder Separator()
        {
            _revitPanel.AddSeparator();
            return this;
        }

        /// <inheritdoc />
        public override void AddAboutButton(
            string name,
            string text,
            string tabName,
            string panelName,
            IContainer container,
            Action<IAboutButtonBuilder> action)
        {
            var ribbon = ComponentManager.Ribbon;
            foreach (var tab in ribbon.Tabs)
            {
                if (tab.Title.Equals(tabName))
                {
                    foreach (var panel in tab.Panels)
                    {
                        if (panel.Source.Title.Equals(panelName))
                        {
                            var button = new AboutButtonBuilder(
                                name,
                                text,
                                Container);
                            action?.Invoke(button);
                            var buttonData = button.BuildButton();
                            panel.Source.Items.Add(buttonData);
                            break;
                        }
                    }

                    break;
                }
            }
        }

        /// <inheritdoc />
        protected override StackedItemsBuilder CreateStackedItems()
        {
            return new StackedItemsBuilder();
        }

        /// <inheritdoc />
        protected override void AddStackedButtonsToRibbon(StackedItemsBuilder stackedItemsBuilder)
        {
            var item1 = stackedItemsBuilder.Buttons[0].GetButtonData();
            var item2 = stackedItemsBuilder.Buttons[1].GetButtonData();
            if (stackedItemsBuilder.ItemsCount == 3)
            {
                var item3 = stackedItemsBuilder.Buttons[2].GetButtonData();
                _revitPanel.AddStackedItems(item1, item2, item3);
            }
            else
            {
                _revitPanel.AddStackedItems(item1, item2);
            }
        }
    }
}