namespace PikTools.Shared.Ui.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for EditableTextBlock.xaml
    /// </summary>
    public partial class EditableTextBlock : UserControl
    {
        /// <summary>
        /// Свойство зависимости для текста
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(EditableTextBlock),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Свойство зависимости для индикатора изменяемости текста
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register(
                "IsEditable",
                typeof(bool),
                typeof(EditableTextBlock),
                new PropertyMetadata(true));

        /// <summary>
        /// Свойство зависимости для индикатора режима изменения
        /// </summary>
        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register(
                "IsInEditMode",
                typeof(bool),
                typeof(EditableTextBlock),
                new PropertyMetadata(false));

        /// <summary>
        /// Свойство зависимости для формата текста
        /// </summary>
        public static readonly DependencyProperty TextFormatProperty =
            DependencyProperty.Register(
                "TextFormat",
                typeof(string),
                typeof(EditableTextBlock),
                new PropertyMetadata("{0}"));

        // We keep the old text when we go into editmode
        // in case the user aborts with the escape key
        private string _oldText;

        /// <summary>
        /// Конструктор
        /// </summary>
        public EditableTextBlock()
        {
            InitializeComponent();
            Focusable = true;
            FocusVisualStyle = null;
        }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Индикатор изменения
        /// </summary>
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        /// <summary>
        /// Индикатор режима изменения
        /// </summary>
        public bool IsInEditMode
        {
            get
            {
                if (IsEditable)
                    return (bool)GetValue(IsInEditModeProperty);
                else
                    return false;
            }
            set
            {
                if (!IsEditable)
                    return;

                if (value)
                    _oldText = Text;

                SetValue(IsInEditModeProperty, value);
            }
        }

        /// <summary>
        /// Формат текста
        /// </summary>
        public string TextFormat
        {
            get => (string)GetValue(TextFormatProperty);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = "{0}";
                SetValue(TextFormatProperty, value);
            }
        }

        /// <summary>
        /// Отформатированный текст
        /// </summary>
        public string FormattedText => string.Format(TextFormat, Text);

        // Invoked when we enter edit mode.
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox txt))
                return;

            // Give the TextBox input focus
            txt.Focus();

            txt.SelectAll();
        }

        // Invoked when we exit edit mode.
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsInEditMode = false;
        }

        // Invoked when the user edits the annotation.
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsInEditMode = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                Text = _oldText;
                IsInEditMode = false;
                e.Handled = true;
            }
        }
    }
}
