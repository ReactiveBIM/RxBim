namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

    /// <summary>
    /// Ribbon events service
    /// </summary>
    public class RibbonEventService : IRibbonEvents, IDisposable
    {
        /// <inheritdoc />
        public event EventHandler? NeedRebuild;

        /// <inheritdoc />
        public void Run()
        {
            Application.SystemVariableChanged += ApplicationOnSystemVariableChanged;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Application.SystemVariableChanged -= ApplicationOnSystemVariableChanged;
        }

        private void ApplicationOnSystemVariableChanged(object sender, SystemVariableChangedEventArgs e)
        {
            const string wsCurrentVariableName = "WSCURRENT";
            if (e.Name.Equals(wsCurrentVariableName))
            {
                Application.Idle += ApplicationOnIdle;
            }
        }

        private void ApplicationOnIdle(object sender, EventArgs e)
        {
            NeedRebuild?.Invoke(this, EventArgs.Empty);
            Application.Idle -= ApplicationOnIdle;
        }
    }
}