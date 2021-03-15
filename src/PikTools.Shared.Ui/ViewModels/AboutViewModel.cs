namespace PikTools.Shared.Ui.ViewModels
{
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Модель представления окна о программе
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="content">Содержимое</param>
        public AboutViewModel(AboutBoxContent content)
        {
            Content = content;
        }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title
            => $"О программе {Content?.Title}";

        /// <summary>
        /// Содержимое
        /// </summary>
        public AboutBoxContent Content { get; }
    }
}
