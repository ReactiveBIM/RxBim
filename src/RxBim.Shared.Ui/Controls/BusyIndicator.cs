namespace RxBim.Shared.Ui.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Область, для которой можно выставить индикатор ожидания
    /// </summary>
    public class BusyIndicator : ContentControl
    {
        /// <summary>
        /// DependencyProperty for <see cref="IsBusy"/>
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register(
                nameof(IsBusy),
                typeof(bool),
                typeof(BusyIndicator));

        /// <summary>
        /// Initializes the <see cref="BusyIndicator"/> class.
        /// </summary>
        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BusyIndicator),
                new FrameworkPropertyMetadata(typeof(BusyIndicator)));
        }

        /// <summary>
        /// Индикатор ожидания
        /// </summary>
        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }
    }
}
