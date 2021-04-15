namespace PikTools.Shared.TableBuilder.Extensions
{
    /// <summary>
    /// Расширения для работы с ячейкой
    /// </summary>
    public static class CellExtensions
    {
        /// <summary>
        /// Установить текст в ячейке
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="text">текст</param>
        public static Cell SetText(this Cell cell, string text)
            => cell.SetValue(new TextCellData(text));

        /// <summary>
        /// Объединить влево, возвращает крайнюю левую ячейку объединения
        /// </summary>
        /// <param name="cell">ячейка</param>
        /// <param name="count">количество ячеек</param>
        public static Cell MergeLeft(this Cell cell, int count = 1)
            => cell.Previous(count).MergeNext(count).Previous(count);
    }
}
