namespace RxBim.Application.Ribbon
{
    using System;
    using Autodesk.Windows;

    /// <summary>
    /// Connects interactive ribbon tooltips to AutoCAD ribbon buttons.
    /// </summary>
    public interface IAutocadEnhancedToolTipService : IDisposable
    {
        /// <summary>
        /// Registers an explicitly configured enhanced tooltip and preserves its native fallback.
        /// </summary>
        /// <param name="button">The AutoCAD ribbon button.</param>
        /// <param name="config">The button configuration.</param>
        /// <param name="parentList">The containing drop-down list, if any.</param>
        void Register(RibbonButton button, Button config, RibbonListButton? parentList = null);

        /// <summary>
        /// Restores native tooltips and releases the registered buttons before rebuilding the ribbon.
        /// </summary>
        void Clear();
    }
}