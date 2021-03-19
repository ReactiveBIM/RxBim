namespace PikTools.Application.Ui.About.ViewModels
{
    using GalaSoft.MvvmLight;
    using PikTools.Shared;

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
