namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Application.Ribbon.Models;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Windows;
    using Extensions;
    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// AutoCAD ribbon button definition
    /// </summary>
    public class Button : ButtonBase
    {
        private readonly string? _command;
        private string? _helpUrl;

        /// <inheritdoc />
        public Button(string name, string text, Type? commandType = null)
            : base(name, text, commandType)
        {
            if (commandType != null)
                _command = commandType.GetCommandName();
        }

        /// <summary>
        /// Show button label text
        /// </summary>
        public bool ShowText { get; set; }

        /// <summary>
        /// Button identifier
        /// </summary>
        internal string Id { get; }

        /// <summary>
        /// Large image for the button in light theme
        /// </summary>
        internal ImageSource? LargeImageLight { get; set; }

        /// <summary>
        /// Small image for the button in light theme
        /// </summary>
        internal ImageSource? SmallImageLight { get; set; }

        /// <summary>
        /// Creates and returns <see cref="RibbonButton"/> for this button definition
        /// </summary>
        public virtual RibbonButton GetRibbonButton()
        {
            var ribbonCmdButton = new RibbonButton
            {
                CommandHandler = new RelayCommand(CommandExecute, true)
            };
            SetBaseProperties(ribbonCmdButton);

            return ribbonCmdButton;
        }

        /// <summary>
        /// Sets basic properties for <see cref="RibbonButton"/> from this button definition
        /// </summary>
        /// <param name="button"><see cref="RibbonButton"/></param>
        protected void SetBaseProperties(RibbonButton button)
        {
            if (Name != null)
                button.Name = Name;
            if (Text != null)
                button.Text = Text;
            if (ToolTip != null)
                button.ToolTip = GetToolTip();
            if (LargeImage != null)
                button.LargeImage = LargeImage;
            if (SmallImage != null)
                button.Image = SmallImage;
            if (Description != null)
                button.Description = Description;
            button.IsCheckable = false;
            button.ShowImage = true;
            button.ShowText = ShowText;
            button.Orientation = Orientation.Vertical;
        }

        /// <inheritdoc />
        protected override void SetHelpUrlInternal(string url)
        {
            _helpUrl = url;
        }

        /// <summary>
        /// Creates and returns a tooltip
        /// </summary>
        private RibbonToolTip GetToolTip()
        {
            var tip = new RibbonToolTip();

            if (ToolTip != null)
                tip.Content = ToolTip;

            if (_helpUrl != null)
            {
                tip.IsHelpEnabled = true;
                tip.HelpTopic = _helpUrl;
            }

            return tip;
        }

        private void CommandExecute()
        {
            Application.DocumentManager.MdiActiveDocument?
                .SendStringToExecute($"{_command} ", false, false, true);
        }
    }
}