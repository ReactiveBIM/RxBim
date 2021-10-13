namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Application.Ribbon.Abstractions;
    using Autodesk.Windows;
    using Di;
    using Extensions;
    using Ribbon.Services;

    /// <summary>
    /// Ribbon panel implementation for AutoCAD-based products
    /// </summary>
    public class PanelBuilder : PanelBuilderBase<RibbonBuilder, StackedItemsBuilder, ButtonBuilder>
    {
        /// <inheritdoc />
        public PanelBuilder(RibbonBuilder ribbonBuilder, IContainer container, string id, ITabBuilder tabBuilder)
            : base(ribbonBuilder, container, tabBuilder)
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
        public override IPanelBuilder Button(
            string name,
            string text,
            Type externalCommandType,
            Action<IButtonBuilder>? action = null)
        {
            var button = new ButtonBuilder(name, text, externalCommandType);
            action?.Invoke(button);
            var cadButton = button.GetRibbonButton();
            cadButton.Size = RibbonItemSize.Large;
            AddToCurrentRow(cadButton);
            return this;
        }

        /// <inheritdoc />
        public override IPanelBuilder PullDownButton(string name, string text, Action<IPulldownButtonBuilder>? action)
        {
            var button = new PulldownButtonBuilder(name, text);
            action?.Invoke(button);
            var cadButton = button.GetRibbonButton();
            cadButton.Size = RibbonItemSize.Large;
            AddToCurrentRow(cadButton);
            return this;
        }

        /// <inheritdoc />
        public override IPanelBuilder Separator()
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
            Action<IAboutButtonBuilder>? action)
        {
            var button = new AboutButtonBuilder(name, text, container);
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
        protected override StackedItemsBuilder CreateStackedItems()
        {
            return new StackedItemsBuilder();
        }

        /// <inheritdoc />
        protected override void AddStackedButtonsToRibbon(StackedItemsBuilder stackedItemsBuilder)
        {
            var rowPanel = new RibbonRowPanel();
            AddToCurrentRow(rowPanel);

            for (var i = 0; i < stackedItemsBuilder.Buttons.Count; i++)
            {
                if (i > 0)
                {
                    rowPanel.Items.Add(new RibbonRowBreak());
                }

                var button = stackedItemsBuilder.Buttons[i];
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