namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using System;

    /// <summary>
    /// Ribbon service
    /// </summary>
    public interface IRibbonEventsService
    {
        /// <summary>
        /// Ribbon need to rebuild
        /// </summary>
        event EventHandler NeedRebuild;

        /// <summary>
        /// Runs the service
        /// </summary>
        void Run();
    }
}