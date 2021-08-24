namespace RxBim.Shared.Ui.Controls
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Поле для поиска
    /// </summary>
    public class SearchBox : TextBox
    {
        /// <summary>
        /// Сво-во зависимости названия поиска, отображаемого в пустой строке
        /// </summary>
        public static readonly DependencyProperty SearchTitleProperty =
            DependencyProperty.Register(
                nameof(SearchTitle),
                typeof(string),
                typeof(SearchBox));

        /// <summary>
        /// Сво-во зависимости коллекции предлагаемых вариантов при поиске
        /// </summary>
        public static readonly DependencyProperty PreviewSearchListProperty =
            DependencyProperty.Register(
                nameof(PreviewSearchList),
                typeof(IEnumerable),
                typeof(SearchBox));

        /// <summary>
        /// Сво-во зависимости для команды поиска
        /// </summary>
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(
                nameof(SearchCommand),
                typeof(ICommand),
                typeof(SearchBox));

        /// <summary>
        /// Initializes the <see cref="SearchBox"/> class.
        /// </summary>
        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }

        /// <summary>
        /// Название поиска, отображаемого в пустой строке
        /// </summary>
        public string SearchTitle
        {
            get => (string)GetValue(SearchTitleProperty);
            set => SetValue(SearchTitleProperty, value);
        }

        /// <summary>
        /// Коллекция предлагаемых вариантов при поиске
        /// </summary>
        public IEnumerable PreviewSearchList
        {
            get => (IEnumerable)GetValue(PreviewSearchListProperty);
            set => SetValue(PreviewSearchListProperty, value);
        }

        /// <summary>
        /// Команда поиска
        /// </summary>
        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }
    }
}
