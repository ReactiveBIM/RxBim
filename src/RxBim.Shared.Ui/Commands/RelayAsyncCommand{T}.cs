namespace RxBim.Shared.Ui.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Abstractions;

    /// <summary>
    /// Асинхронная команда с параметром
    /// </summary>
    /// <typeparam name="T">Тип параметра</typeparam>
    public class RelayAsyncCommand<T> : IAsyncCommand<T>
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;

        private bool _isExecuting;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="execute">Метод выполнения комнады</param>
        /// <param name="canExecute">Метод проверки возможности выполнения команды</param>
        public RelayAsyncCommand(
            Func<T, Task> execute,
            Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Событие проверки возможности выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Возможность выполнить команду
        /// </summary>
        /// <param name="parameter">Параметр</param>
        /// <returns>true - возможно, иначе - false</returns>
        public bool CanExecute(T parameter)
        {
            return !_isExecuting
                && (_canExecute?.Invoke(parameter) ?? true);
        }

        /// <summary>
        /// Выполнение асинхронной команды
        /// </summary>
        /// <param name="parameter">Параметр</param>
        public async Task ExecuteAsync(T parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Вызов события проверки возможности выполнения команды
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        /// <inheritdoc/>
        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }
    }
}
