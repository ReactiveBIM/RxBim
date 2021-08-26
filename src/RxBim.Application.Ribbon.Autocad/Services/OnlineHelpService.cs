namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.Internal.Windows;
    using Autodesk.Windows;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <summary>
    /// Сервис обработки онлайн-справки
    /// </summary>
    /// <remarks>
    /// https://forums.autodesk.com/t5/net/getting-ribbon-help-to-call-html/m-p/4943910
    /// </remarks>
    public class OnlineHelpService : IDisposable, IOnlineHelpService
    {
        // Flag to tell if the next message from AutoCAD to display it's own help should be ignored
        private static bool _dropNextHelpCall;

        // If not null, this is the HelpTopic of the currently open tooltip. If null, no tooltip is displaying.
        private static string _helpTopic;

        /// <summary>
        /// Сообщения
        /// </summary>
        public enum WndMsg
        {
            /// <summary>
            /// Справка
            /// </summary>
            WmAcadHelp = 0x4D,

            /// <summary>
            /// Нажата клавиша
            /// </summary>
            WmKeyDown = 0x100,
        }

        /// <summary>
        /// Ключи клавиш
        /// </summary>
        public enum WndKey
        {
            /// <summary>
            /// Клавиша F1
            /// </summary>
            KeyF1 = 0x70,
        }

        /// <summary>
        /// Запускает работу сервиса
        /// </summary>
        public void Run()
        {
            Application.PreTranslateMessage += AutoCadMessageHandler;
            ComponentManager.ToolTipOpened += ComponentManager_ToolTipOpened;
            ComponentManager.ToolTipClosed += ComponentManager_ToolTipClosed;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Application.PreTranslateMessage -= AutoCadMessageHandler;
            ComponentManager.ToolTipOpened -= ComponentManager_ToolTipOpened;
            ComponentManager.ToolTipClosed -= ComponentManager_ToolTipClosed;
        }

        // AutoCAD event handlers to detect if a tooltip is open or not
        private static void ComponentManager_ToolTipOpened(object sender, EventArgs e)
        {
            if (sender is ToolTip tt)
            {
                _helpTopic = tt.Content is RibbonToolTip rtt ? rtt.HelpTopic : tt.HelpTopic;
            }
        }

        private static void ComponentManager_ToolTipClosed(object sender, EventArgs e)
        {
            _helpTopic = null;
        }

        private void AutoCadMessageHandler(object sender, PreTranslateMessageEventArgs e)
        {
            if (e.Message.message == (int)WndMsg.WmKeyDown)
            {
                if ((int)e.Message.wParam == (int)WndKey.KeyF1 &&
                    _helpTopic != null &&
                    Uri.IsWellFormedUriString(_helpTopic, UriKind.Absolute))
                {
                    // Another implementation could be to look up the help topic in an index file matching it to URLs.
                    // Even though we don't forward this F1 keypress, AutoCAD sends a message to itself to open the AutoCAD help file
                    _dropNextHelpCall = true;
                    const string noMuttVariableName = "NOMUTT";
                    const int offMsgVariableValue = 1;
                    var noMutt = Application.GetSystemVariable(noMuttVariableName);
                    Application.SetSystemVariable(noMuttVariableName, offMsgVariableValue);
                    var cmd = $"._BROWSER {_helpTopic} _{noMuttVariableName} {noMutt} ";
                    Application.DocumentManager.MdiActiveDocument
                        .SendStringToExecute(cmd, true, false, false);
                    e.Handled = true;
                }
            }
            else if (e.Message.message == (int)WndMsg.WmAcadHelp && _dropNextHelpCall)
            {
                // Seems this is the message AutoCAD generates itself to open the help file. Drop this if help was called from a ribbon tooltip.
                _dropNextHelpCall = false; // Reset state of help calls
                e.Handled = true; // Stop this message from being passed on to AutoCAD
            }
        }
    }
}