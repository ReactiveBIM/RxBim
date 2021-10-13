namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Application.Ribbon.Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Windows;
    using Di;
    using Extensions;

    /// <summary>
    /// Ribbon panel implementation for AutoCAD-based products
    /// </summary>
    public class Panel : PanelBase<Ribbon, StackedItems, Button>
    {
        /// <inheritdoc />
        public Panel(Ribbon ribbon, IContainer container, string id, ITab tab)
            : base(ribbon, container, tab)
        {
            Id = id;
            var acadPanel = this.GetRibbonPanel();

            var mainPanelRow = acadPanel.Source.Items.FirstOrDefault(x => x is RibbonRowPanel);
            if (mainPanelRow is null)
            {
                mainPanelRow = new RibbonRowPanel();
                acadPanel.Source.Items.Add(mainPanelRow);
            }
        }

        /// <summary>
        /// Ribbon panel control identifier
        /// </summary>
        public string Id { get; }

        /// <inheritdoc />
        public override IPanel Button(
            string name,
            string text,
            Type externalCommandType,
            Action<IButton>? action = null)
        {
            var button = new Button(name, text, externalCommandType);
            action?.Invoke(button);
            var cadButton = button.GetRibbonButton();
            cadButton.Size = RibbonItemSize.Large;
            AddToCurrentRow(cadButton);
            return this;
        }

        /// <inheritdoc />
        public override IPanel PullDownButton(string name, string text, Action<IPulldownButton>? action)
        {
            var button = new PulldownButton(name, text);
            action?.Invoke(button);
            var cadButton = button.GetRibbonButton();
            cadButton.Size = RibbonItemSize.Large;
            AddToCurrentRow(cadButton);
            return this;
        }

        /// <inheritdoc />
        public override IPanel Separator()
        {
            AddToCurrentRow(new RibbonSeparator());
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
            AddToCurrentRow(cadButton);
        }

        /// <summary>
        /// Switches the panel to SlideOut fill mode
        /// </summary>
        internal void SwitchToSlideOut()
        {
            var ribbonPanel = this.GetRibbonPanel();
            if (ribbonPanel.Source.Items.Any(x => x is RibbonPanelBreak))
                return;

            ribbonPanel.Source.Items.Add(new RibbonPanelBreak());
            ribbonPanel.Source.Items.Add(new RibbonRowPanel());
        }

        /// <inheritdoc />
        protected override StackedItems CreateStackedItems()
        {
            return new StackedItems();
        }

        /// <inheritdoc />
        protected override void AddStackedButtonsToRibbon(StackedItems stackedItems)
        {
            var rowPanel = new RibbonRowPanel();
            AddToCurrentRow(rowPanel);

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

        private void AddToCurrentRow(RibbonItem item)
        {
            var row = (RibbonRowPanel)this.GetRibbonPanel().Source.Items.Last(x => x is RibbonRowPanel);
            row.Items.Add(item);
        }
    }
}