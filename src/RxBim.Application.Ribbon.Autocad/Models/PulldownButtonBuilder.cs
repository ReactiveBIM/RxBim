namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Application.Ribbon.Abstractions;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;

    /// <summary>
    /// PulldownButton for AutoCAD Ribbon
    /// </summary>
    public class PulldownButtonBuilder : ButtonBuilder, IPulldownButtonBuilder
    {
        private readonly IList<ButtonBuilder> _buttons = new List<ButtonBuilder>();

        /// <inheritdoc />
        public PulldownButtonBuilder(string name, string text)
            : base(name, text)
        {
        }

        /// <inheritdoc />
        public IPulldownButtonBuilder Button(string name, string text, Type externalCommandType)
        {
            return Button(name, text, externalCommandType, null);
        }

        /// <inheritdoc />
        public IPulldownButtonBuilder Button(string name, string text, Type externalCommandType, Action<IButtonBuilder>? action)
        {
            var button = new ButtonBuilder(name, text, externalCommandType);
            action?.Invoke(button);
            _buttons.Add(button);
            return this;
        }

        /// <inheritdoc />
        public override RibbonButton GetRibbonButton()
        {
            var splitButton = new RibbonSplitButton
            {
                Orientation = Orientation.Vertical,
                ListButtonStyle = RibbonListButtonStyle.SplitButton,
                ResizeStyle = RibbonItemResizeStyles.NoResize,
                ListStyle = RibbonSplitButtonListStyle.List
            };

            SetBaseProperties(splitButton);

            foreach (var button in _buttons)
            {
                splitButton.Items.Add(button.GetRibbonButton());
            }

            return splitButton;
        }
    }
}