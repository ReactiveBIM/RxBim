namespace RxBim.Application.Revit
{
    using System;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Proxy class for adding UIApplication to the container.
    /// The main reason is that some actions, such as the ribbon creation, must be performed at startup, when UIApplication is not available yet.
    /// </summary>
    internal class UIApplicationProxy
    {
        private UIApplication? _uiApp;

        /// <summary>
        /// Check whether this instance is already initialized.
        /// </summary>
        public bool IsInitialized => _uiApp != null;

        /// <inheritdoc cref="Autodesk.Revit.UI.UIApplication"/>
        public UIApplication UIApplication => _uiApp
            ?? throw new InvalidOperationException("Revit initialization sequence is in progress. Revit API is not fully available yet.");

        /// <summary>
        /// Initialize proxy with <see cref="UIApplication"/>.
        /// </summary>
        /// <param name="uIApplication">Session of the Autodesk Revit user interface.</param>
        public void Initialize(UIApplication uIApplication)
        {
            if (!IsInitialized)
                _uiApp = uIApplication;
        }
    }
}