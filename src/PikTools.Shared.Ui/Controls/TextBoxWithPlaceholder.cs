namespace PikTools.Shared.Ui.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// TextBox с текстом по умолчанию
    /// </summary>
    public class TextBoxWithPlaceholder : TextBox
    {
        /// <summary>
        /// Сво-во зависимости текста по-умолчанию
        /// </summary>
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                nameof(Placeholder),
                typeof(string),
                typeof(TextBoxWithPlaceholder));

        /// <summary>
        /// Initializes the <see cref="TextBoxWithPlaceholder"/> class.
        /// </summary>
        static TextBoxWithPlaceholder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextBoxWithPlaceholder),
                new FrameworkPropertyMetadata(typeof(TextBoxWithPlaceholder)));
        }

        /// <summary>
        /// Текст по-умолчанию
        /// </summary>
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
    }
}
