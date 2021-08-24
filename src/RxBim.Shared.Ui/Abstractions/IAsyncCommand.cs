namespace RxBim.Shared.Ui.Abstractions
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Интерфейс асинхронной команды
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Выполнение команды
        /// </summary>
        Task ExecuteAsync();

        /// <summary>
        /// Возможность выполнения команды
        /// </summary>
        /// <returns>true - возмжно. иначе - false</returns>
        bool CanExecute();
    }

    /// <summary>
    /// Интерфейс асинхронной команды с параметром
    /// </summary>
    /// <typeparam name="T">Тип параметра</typeparam>
    public interface IAsyncCommand<T> : ICommand
    {
        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="parameter">Параметр</param>
        Task ExecuteAsync(T parameter);

        /// <summary>
        /// Возможность выполнения команды
        /// </summary>
        /// <param name="parameter">Параметр</param>
        /// <returns>true - возмжно. иначе - false</returns>
        bool CanExecute(T parameter);
    }
}
