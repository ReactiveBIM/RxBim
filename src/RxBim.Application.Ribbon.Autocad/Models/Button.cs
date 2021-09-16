namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Reflection;
    using System.Windows.Controls;
    using Application.Ribbon.Models;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Windows;
    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// AutoCAD ribbon button definition
    /// </summary>
    public class Button : ButtonBase
    {
        private readonly string _command;
        private string _helpUrl;

        /// <inheritdoc />
        public Button(string name, string text, Type externalCommandType = null)
            : base(name, text, externalCommandType)
        {
            if (externalCommandType != null)
                _command = GetCommandName(externalCommandType);
        }

        /// <summary>
        /// Show button text
        /// </summary>
        public bool ShowText { get; set; }

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
            tip.IsHelpEnabled = true;
            if (_helpUrl != null)
                tip.HelpTopic = _helpUrl;
            return tip;
        }

        private void CommandExecute()
        {
            Application.DocumentManager.MdiActiveDocument?
                .SendStringToExecute($"{_command} ", false, false, true);
        }

        private string GetCommandName(MemberInfo externalCommandType)
        {
            const string cmdNameProperty = "CommandName";
            var attributes = Attribute.GetCustomAttributes(externalCommandType);

            foreach (var attribute in attributes)
            {
                try
                {
                    var cmdProperty = attribute.GetType()
                        .GetProperty(cmdNameProperty,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

                    if (cmdProperty is null)
                        continue;

                    return cmdProperty.GetValue(attribute).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new InvalidOperationException("Failed to retrieve command name!", e);
                }
            }

            return externalCommandType.Name;
        }
    }
}