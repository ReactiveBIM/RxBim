namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using Di;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Панель revit
    /// </summary>
    public class Panel : PanelBase<Ribbon, StackedItem, Button>
    {
        private readonly RibbonPanel _revitPanel;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon"><see cref="Ribbon"/></param>
        /// <param name="revitPanel"><see cref="RibbonPanel"/></param>
        /// <param name="container"><see cref="IContainer"/></param>
        public Panel(Ribbon ribbon, RibbonPanel revitPanel, IContainer container)
            : base(ribbon, container)
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
        public override IPanel Button(string name, string text, Type externalCommandType, Action<IButton> action = null)
        {
            var button = new Button(name, text, externalCommandType);
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
        public override IPanel PullDownButton(string name, string text, Action<IPulldownButton> action)
        {
            var button = new PulldownButton(name, text);
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
        public override IPanel Separator()
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
            Action<IAboutButton> action)
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
                            var button = new AboutButton(
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
        protected override StackedItem CreateStackedItem()
        {
            return new StackedItem();
        }

        /// <inheritdoc />
        protected override void AddStackedItemButtonsToRibbon(StackedItem stackedItem)
        {
            var item1 = stackedItem.Buttons[0].GetButtonData();
            var item2 = stackedItem.Buttons[1].GetButtonData();
            if (stackedItem.ItemsCount == 3)
            {
                var item3 = stackedItem.Buttons[2].GetButtonData();
                _revitPanel.AddStackedItems(item1, item2, item3);
            }
            else
            {
                _revitPanel.AddStackedItems(item1, item2);
            }
        }
    }
}