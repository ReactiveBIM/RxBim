namespace PikTools.Shared.Ui.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using PikTools.Shared.Ui.Abstractions;

    /// <summary>
    /// Диспетчер UI потока
    /// </summary>
    public class UIDispatcher : IUIDispatcher
    {
        private readonly Dispatcher _uiDispatcher = Dispatcher.CurrentDispatcher;

        /// <inheritdoc />
        public void Invoke(Action action)
        {
            if (action == null)
                return;

            _uiDispatcher?.Invoke(action);
        }

        /// <inheritdoc />
        public async Task InvokeAsync(Action action)
        {
            if (action == null)
                return;

            await _uiDispatcher?.InvokeAsync(action);
        }
    }
}
