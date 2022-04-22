namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Ribbon service.
    /// </summary>
    public interface IRibbonEventsService
    {
        /// <summary>
        /// Raises when the ribbon needs to rebuild.
        /// </summary>
        event EventHandler NeedRebuild;

        /// <summary>
        /// Starts the service.
        /// </summary>
        void Run();
    }
}