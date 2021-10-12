namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Application.Ribbon.Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Windows;
    using Di;

    /// <summary>
    /// Панель
    /// </summary>
    public class Panel : PanelBase<Ribbon, StackedItems, Button>
    {
        /// <inheritdoc />
        public Panel(Ribbon ribbon, IContainer container, string id, ITab tab)
            : base(ribbon, container, id, tab)
        {
            _currentPanelRow = acadPanel.Source.Items.FirstOrDefault(x => x is RibbonRowPanel) as RibbonRowPanel;

            if (_currentPanelRow is null)
            {
                _currentPanelRow = new RibbonRowPanel();
                acadPanel.Source.Items.Add(_currentPanelRow);
            }
        }

        /// <inheritdoc />
        public override IPanel Button(string name, string text, Type externalCommandType, Action<IButton>? action = null)
        {
            var button = new Button(name, text, externalCommandType);
            action?.Invoke(button);
            var cadButton = button.GetRibbonButton();
            cadButton.Size = RibbonItemSize.Large;
            _currentPanelRow?.Items.Add(cadButton);
            return this;
        }

        /// <inheritdoc />
        public override IPanel PullDownButton(string name, string text, Action<IPulldownButton>? action)
        {
            var button = new PulldownButton(name, text);
            action?.Invoke(button);
            var cadButton = button.GetRibbonButton();
            cadButton.Size = RibbonItemSize.Large;
            _currentPanelRow?.Items.Add(cadButton);
            return this;
        }

        /// <inheritdoc />
        public override IPanel Separator()
        {
            _currentPanelRow?.Items.Add(new RibbonSeparator());
            return this;
        }

        /// <inheritdoc />
        public override void AddAboutButton(
            string name,
            string text,
            string tabName,
            string panelName,
            IContainer container,
            Action<IAboutButton>? action)
        {
            var button = new AboutButton(name, text, container);
            action?.Invoke(button);
            var cadButton = button.BuildButton();
            _currentPanelRow?.Items.Add(cadButton);
        }

        /// <summary>
        /// Переключает панель на заполнение её выдвигающейся части
        /// </summary>
        internal void SwitchToSlideOut()
        {
            _currentPanelRow = _ribbonPanel.Source?.Items
                .SkipWhile(x => x is not RibbonPanelBreak)
                .FirstOrDefault(x => x is RibbonRowPanel) as RibbonRowPanel;

            if (_currentPanelRow is null)
            {
                _currentPanelRow = new RibbonRowPanel();
                _ribbonPanel.Source?.Items.Add(new RibbonPanelBreak());
                _ribbonPanel.Source?.Items.Add(_currentPanelRow);
            }
        }

        /// <inheritdoc />
        protected override StackedItems CreateStackedItems()
        {
            return new ();
        }

        /// <inheritdoc />
        protected override void AddStackedButtonsToRibbon(StackedItems stackedItems)
        {
            var rowPanel = new RibbonRowPanel();
            _currentPanelRow?.Items.Add(rowPanel);

            for (var i = 0; i < stackedItems.Buttons.Count; i++)
            {
                if (i > 0)
                {
                    rowPanel.Items.Add(new RibbonRowBreak());
                }

                var button = stackedItems.Buttons[i];
                var cadButton = button.GetRibbonButton();
                cadButton.Size = RibbonItemSize.Standard;
                cadButton.Orientation = Orientation.Horizontal;
                rowPanel.Items.Add(cadButton);
            }
        }
    }
}