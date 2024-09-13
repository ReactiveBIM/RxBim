﻿namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Collections.Generic;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.Internal.Windows;
    using Autodesk.Windows;
    using static AutocadMenuConstants;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <summary>
    /// Online help processing service.
    /// </summary>
    /// <remarks>
    /// https://forums.autodesk.com/t5/net/getting-ribbon-help-to-call-html/m-p/4943910.
    /// </remarks>
    internal class OnlineHelpService : IDisposable, IOnlineHelpService
    {
        private readonly HashSet<RibbonToolTip> _trackedToolTips = new();
        private bool _dropNextHelpCall;
        private string? _helpTopic;

        /// <summary>
        /// Messages.
        /// </summary>
        private enum Messages
        {
            /// <summary>
            /// AutoCAD help call message.
            /// </summary>
            AcadHelp = 0x4D,

            /// <summary>
            /// Key down message.
            /// </summary>
            KeyDown = 0x100,
        }

        /// <summary>
        /// Keys.
        /// </summary>
        private enum Keys
        {
            /// <summary>
            /// F1 key.
            /// </summary>
            F1 = 0x70,
        }

        /// <inheritdoc />
        public void Run()
        {
            Application.PreTranslateMessage += AutoCadMessageHandler;
            ComponentManager.ToolTipOpened += ComponentManager_ToolTipOpened;
            ComponentManager.ToolTipClosed += ComponentManager_ToolTipClosed;
        }

        /// <inheritdoc />
        public void AddToolTip(RibbonToolTip toolTip)
        {
            _trackedToolTips.Add(toolTip);
        }

        /// <inheritdoc />
        public void ClearToolTipsCache()
        {
            _trackedToolTips.Clear();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Application.PreTranslateMessage -= AutoCadMessageHandler;
            ComponentManager.ToolTipOpened -= ComponentManager_ToolTipOpened;
            ComponentManager.ToolTipClosed -= ComponentManager_ToolTipClosed;
        }

        private void ComponentManager_ToolTipOpened(object? sender, EventArgs e)
        {
            if (sender is ToolTip { Content: RibbonToolTip ribbonToolTip } && _trackedToolTips.Contains(ribbonToolTip))
                _helpTopic = ribbonToolTip.HelpTopic;
        }

        private void ComponentManager_ToolTipClosed(object? sender, EventArgs e)
        {
            _helpTopic = null;
        }

        private void AutoCadMessageHandler(object? sender, PreTranslateMessageEventArgs e)
        {
            if (e.Message.message == (int)Messages.KeyDown)
            {
                if ((int)e.Message.wParam != (int)Keys.F1 || _helpTopic == null ||
                    !Uri.IsWellFormedUriString(_helpTopic, UriKind.Absolute))
                {
                    return;
                }

                _dropNextHelpCall = true;
                var currentMuterringState = Application.GetSystemVariable(MuterringVariableName);
                Application.SetSystemVariable(MuterringVariableName, MuterringOffValue);
                var cmd = $"._BROWSER {_helpTopic} _{MuterringVariableName} {currentMuterringState} ";
                Application.DocumentManager.MdiActiveDocument.SendStringToExecute(cmd, true, false, false);
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