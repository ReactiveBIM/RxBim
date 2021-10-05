namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.Internal.Windows;
    using Autodesk.Windows;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <summary>
    /// Online help processing service
    /// </summary>
    /// <remarks>
    /// https://forums.autodesk.com/t5/net/getting-ribbon-help-to-call-html/m-p/4943910
    /// </remarks>
    public class OnlineHelpService : IDisposable, IOnlineHelpService
    {
        private static bool _dropNextHelpCall;
        private static string _helpTopic;

        /// <summary>
        /// Messages
        /// </summary>
        private enum Messages
        {
            /// <summary>
            /// AutoCAD help call message
            /// </summary>
            AcadHelp = 0x4D,

            /// <summary>
            /// Key down message
            /// </summary>
            KeyDown = 0x100,
        }

        /// <summary>
        /// Keys
        /// </summary>
        private enum Keys
        {
            /// <summary>
            /// F1 key
            /// </summary>
            F1 = 0x70,
        }

        /// <summary>
        /// Starts the service
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
            if (e.Message.message == (int)Messages.KeyDown)
            {
                if ((int)e.Message.wParam != (int)Keys.F1 ||
                    _helpTopic == null ||
                    !Uri.IsWellFormedUriString(_helpTopic, UriKind.Absolute))
                {
                    return;
                }

                _dropNextHelpCall = true;
                const string varName = "NOMUTT";
                const short offMsg = 1;
                var oldValue = Application.GetSystemVariable(varName);
                Application.SetSystemVariable(varName, offMsg);
                var cmd = $"._BROWSER {_helpTopic} _{varName} {oldValue} ";
                Application.DocumentManager.MdiActiveDocument
                    .SendStringToExecute(cmd, true, false, false);
                e.Handled = true;
            }
            else if (e.Message.message == (int)Messages.AcadHelp && _dropNextHelpCall)
            {
                _dropNextHelpCall = false;
                e.Handled = true;
            }
        }
    }
}