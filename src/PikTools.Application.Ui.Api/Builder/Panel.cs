namespace PikTools.Application.Ui.Api.Builder
{
    using System;
    using Autodesk.Revit.UI;
    using PikTools.Di;

    /// <summary>
    /// Панель revit
    /// </summary>
    public class Panel : RibbonBuilder
    {
        private readonly Tab _tab;
        private readonly RibbonPanel _revitPanel;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon"><see cref="Ribbon"/></param>
        /// <param name="tab"><see cref="Tab"/></param>
        /// <param name="revitPanel"><see cref="RibbonPanel"/></param>
        /// <param name="container"><see cref="IContainer"/></param>
        public Panel(Ribbon ribbon, Tab tab, RibbonPanel revitPanel, IContainer container)
            : base(ribbon, container)
        {
            _tab = tab;
            _revitPanel = revitPanel;
        }

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        public Panel StackedItems(Action<StackedItem> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            StackedItem stackedItem = new StackedItem(this);

            action.Invoke(stackedItem);

            if (stackedItem.ItemsCount < 2 || stackedItem.ItemsCount > 3)
            {
                throw new InvalidOperationException("You must create 2 or three items in the StackedItems");
            }

            var item1 = stackedItem.Buttons[0].Finish();
            var item2 = stackedItem.Buttons[1].Finish();
            if (stackedItem.ItemsCount == 3)
            {
                var item3 = stackedItem.Buttons[2].Finish();
                _revitPanel.AddStackedItems(item1, item2, item3);
            }
            else
            {
                _revitPanel.AddStackedItems(item1, item2);
            }

            return this;
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <returns>Panel where button were created</returns>
        public Panel Button<TExternalCommandClass>(string name, string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            return Button<TExternalCommandClass>(name, text, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public Panel Button<TExternalCommandClass>(string name, string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            var commandClassType = typeof(TExternalCommandClass);
            return Button(name, text, commandClassType, action);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface. 
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        public Panel Button(string name, string text, Type externalCommandType)
        {
            return Button(name, text, externalCommandType, null);
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
        public Panel Button(string name, string text, Type externalCommandType, Action<Button> action)
        {
            Button button = new Button(name, text, externalCommandType);
            action?.Invoke(button);

            var buttonData = button.Finish();

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
        public Panel PullDownButton(string name, string text, Action<PulldownButton> action)
        {
            PulldownButton button = new PulldownButton(name,
                text);

            action?.Invoke(button);

            var buttonData = button.Finish();

            var ribbonItem = _revitPanel.AddItem(buttonData) as Autodesk.Revit.UI.PulldownButton;

            button.BuildButtons(ribbonItem);

            return this;
        }

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        public Panel Separator()
        {
            _revitPanel.AddSeparator();
            return this;
        }
    }
}