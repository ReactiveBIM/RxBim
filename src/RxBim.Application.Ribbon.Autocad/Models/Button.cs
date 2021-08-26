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
    /// Кнопка
    /// </summary>
    public class Button : ButtonBase
    {
        /// <summary>
        /// Команда
        /// </summary>
        private readonly string _command;

        /// <summary>
        /// Ссылка на справку
        /// </summary>
        private string _helpUrl;

        /// <inheritdoc />
        public Button(string name, string text, Type externalCommandType = null)
            : base(name, text, externalCommandType)
        {
            if (externalCommandType != null)
                _command = GetCommandName(externalCommandType);
        }

        /// <summary>
        /// Отображать текст
        /// </summary>
        public bool ShowText { get; set; }

        /// <summary>
        /// Возвращает RibbonButton
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
        /// Задаёт базовые свойства кнопке
        /// </summary>
        /// <param name="button">Кнопка</param>
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
        /// Создаёт подсказку
        /// </summary>
        private RibbonToolTip GetToolTip()
        {
            var tip = new RibbonToolTip();
            if (ToolTip != null)
                tip.Content = ToolTip;
            tip.IsHelpEnabled = true;
            if (_helpUrl != null)
                tip.HelpSource = new Uri(_helpUrl, UriKind.Absolute);
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
                    throw new InvalidOperationException("Не удалось извлечь имя команды!", e);
                }
            }

            return externalCommandType.Name;
        }
    }
}