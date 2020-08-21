namespace PikTools.Shared.Ui.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Базовая модель представления
    /// </summary>
    public abstract class MainViewModelBase : ViewModelBase
    {
        private readonly string _pluginName;
        private string _title;

        /// <summary>
        /// Защищенный конструктор
        /// </summary>
        /// <param name="pluginName">Название плагина</param>
        protected MainViewModelBase(string pluginName)
        {
            _pluginName = pluginName;
        }

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    var testSuffix = string.Empty;

#if DEBUG
                    testSuffix = "TEST";
#endif

                    _title =
                        $"{_pluginName} - {GetType().Assembly.GetName().Version}{(string.IsNullOrEmpty(testSuffix) ? string.Empty : $" ({testSuffix})")}";
                }

                return _title;
            }
        }

        /// <summary>
        /// Команда инициализации модели представления
        /// </summary>
        public ICommand InitializeCommand { get; protected set; }
    }
}
