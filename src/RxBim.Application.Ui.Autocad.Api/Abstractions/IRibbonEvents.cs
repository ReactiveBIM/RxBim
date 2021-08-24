namespace RxBim.Application.Ui.Autocad.Api.Abstractions
{
    using System;

    /// <summary>
    /// События ленты
    /// </summary>
    public interface IRibbonEvents
    {
        /// <summary>
        /// Лента была перестроена
        /// </summary>
        event EventHandler NeedRebuild;

        /// <summary>
        /// Запуск сервиса
        /// </summary>
        void Run();
    }
}