namespace RxBim.Application.Ribbon
{
    using System;

    /// <summary>
    /// Ribbon service.
    /// </summary>
    public interface IRibbonEventsService
    {
        /// <summary>
        /// Ribbon need to rebuild
        /// </summary>
        event EventHandler NeedRebuild;

        /// <summary>
        /// Starts the service.
        /// </summary>
        void Run();
    }
}