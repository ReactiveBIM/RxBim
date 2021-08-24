namespace RxBim.Shared.Ui.Abstractions
{
    using System;
    using System.Threading.Tasks;

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
        Task InvokeAsync(Action action);
    }
}
