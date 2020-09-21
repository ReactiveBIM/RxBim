namespace PikTools.Shared.Ui.Abstractions
{
    using System;

    /// <summary>
    /// Интерфейс диспетчера UI потока
    /// </summary>
    public interface IUIDispatcher
    {
        /// <summary>
        /// Выполняет действие в UI потоке
        /// </summary>
        /// <param name="action">Действие</param>
        void Invoke(Action action);

        /// <summary>
        /// Выполнить асинхронное действие в UI потоке
        /// </summary>
        /// <param name="action">Действие</param>
        void InvokeAsync(Action action);
    }
}
