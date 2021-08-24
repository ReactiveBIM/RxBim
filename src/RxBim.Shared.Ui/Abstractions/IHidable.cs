namespace RxBim.Shared.Ui.Abstractions
{
    /// <summary>
    /// Дает возможность скрыть окно
    /// </summary>
    public interface IHidable : IClosable
    {
        /// <summary>
        /// Скрыть окно
        /// </summary>
        void Hide();

        /// <summary>
        /// Показать окно
        /// </summary>
        void Show();

        /// <summary>
        /// Показать диалоговое окно
        /// </summary>
        bool? ShowDialog();
    }
}
