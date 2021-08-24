namespace RxBim.Shared.Ui.ViewModels
{
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Модель представления оборачивающая тип для возможности выбора
    /// </summary>
    /// <typeparam name="T">Тип, который необходимо обернуть индикатором выбора</typeparam>
    public class SelectableViewModel<T> : ViewModelBase
    {
        private T _value;
        private bool _isChecked;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="value">Значение, обернутое выбором</param>
        /// <param name="isChecked">Индикатор выбора</param>
        public SelectableViewModel(
            T value,
            bool isChecked)
        {
            _value = value;
            _isChecked = isChecked;
        }

        /// <summary>
        /// Значение, обернутое выбором
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Индикатор выбора
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }
    }
}
