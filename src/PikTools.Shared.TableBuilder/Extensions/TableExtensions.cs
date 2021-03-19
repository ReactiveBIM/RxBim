namespace PikTools.Shared.TableBuilder.Extensions
{
    using System.Linq;

    /// <summary>
    /// Расширения для работы с таблицей
    /// </summary>
    public static class TableExtensions
    {
        /// <summary>
        /// Установить формат таблицы по ГОСТу
        /// <para>Весь текст по центру, все линии шапки - толстые</para>
        /// <para>Все горизонтальные границы между строками таблицы - стандартной толщины</para>
        /// </summary>
        /// <param name="table">Таблица</param>
        /// <param name="headerSize">Размер шапки таблицы</param>
        public static Table SetTableStateStandardFormat(this Table table, int headerSize)
        {
            var boldFormat = new CellFormatStyle
            {
                TextVerticalAlignment = TextVerticalAlignment.Middle,
                Borders = new CellBorders(CellBorderType.Bold, CellBorderType.Bold, CellBorderType.Bold, CellBorderType.Bold)
            };

            var rowFormat = new CellFormatStyle
            {
                TextVerticalAlignment = TextVerticalAlignment.Middle,
                Borders = new CellBorders(CellBorderType.Usual, CellBorderType.Usual, CellBorderType.Bold, CellBorderType.Bold)
            };

            var lastRowFormat = new CellFormatStyle
            {
                TextVerticalAlignment = TextVerticalAlignment.Middle,
                Borders = new CellBorders(CellBorderType.Usual, CellBorderType.Bold, CellBorderType.Bold, CellBorderType.Bold)
            };

            table.SetFormat(boldFormat);

            if (table.Rows.Count > headerSize + 1)
            {
                table.SetFormat(rowFormat, headerSize + 1, 0, table.Columns.Count, table.Rows.Count - headerSize);
                foreach (var cell in table.Rows.Last().Cells)
                    cell.SetFormat(lastRowFormat);
            }

            return table;
        }
    }
}
