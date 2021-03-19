namespace PikTools.Shared.TableBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PikTools.Shared.TableBuilder.Abstractions;

    /// <summary> Table </summary>
    public class Table
    {
        /// <summary> ctor </summary>
        /// <param name="width">Ширина таблицы</param>
        public Table(double? width = null)
        {
            Rows = new List<Row>();
            Columns = new List<Column>();
            Width = width;
        }

        /// <summary> Строки </summary>
        public List<Row> Rows { get; }

        /// <summary> Столбцы </summary>
        public List<Column> Columns { get; }

        /// <summary> Ширина таблицы </summary>
        public double? Width { get; }

        /// <summary>
        /// Возвращает элемент таблицы по индексу
        /// Бросает исключение, если объект не найден
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="cell">Номер ячейки</param>
        public Cell this[int row, int cell] => FindByIndex(row, cell);

        /// <summary>
        /// Создает строки
        /// </summary>
        /// <param name="action">Действие над строкой</param>
        /// <param name="count">Количество строк</param>
        public Table AddRow(Action<Row> action = null, int count = 1)
        {
            for (; count > 0; count--)
            {
                var newRow = new Row(this);

                foreach (var column in Columns)
                {
                    var cell = new Cell(this, newRow, column);
                    cell.SetFormat(column.DefaultCellFormat);
                    newRow.Cells.Add(cell);
                    column.Cells.Add(cell);
                }

                Rows.Add(newRow);
                action?.Invoke(newRow);
            }

            return this;
        }

        /// <summary>
        /// Создает столбец во всех строках
        /// </summary>
        /// <param name="action">Делегат, применяется ко всему создаваемому столбцу</param>
        /// <param name="count">Количество</param>
        public Table AddColumn(Action<Column> action = null, int count = 1)
        {
            for (; count > 0; count--)
            {
                var newColumn = new Column(this);

                foreach (var row in Rows)
                {
                    var cell = new Cell(this, row, newColumn);
                    newColumn.Cells.Add(cell);
                    row.Cells.Add(cell);
                }

                Columns.Add(newColumn);
                action?.Invoke(newColumn);
            }

            return this;
        }

        /// <summary>
        /// Применить форматирование ко всем ячейкам таблицы, это перезапишет формат ячеек установленный ранее
        /// </summary>
        /// <param name="format">Формат</param>
        public Table SetFormat(CellFormatStyle format)
        {
            foreach (var column in Columns)
            {
                column.SetFormat(format);
                foreach (var cell in column.Cells)
                    cell.SetFormat(format);
            }

            return this;
        }

        /// <summary>
        /// Применить форматирование ко всем ячейкам диапазона таблицы
        /// </summary>
        /// <param name="format">Формат</param>
        /// <param name="startRow">Стартовая строка</param>
        /// <param name="startCol">Стартовая колонка</param>
        /// <param name="width">Ширина диапазона</param>
        /// <param name="height">Высота диапазона</param>
        public Table SetFormat(CellFormatStyle format, int startRow, int startCol, int width, int height)
        {
            foreach (var column in Columns.Skip(startCol).Take(width))
            {
                foreach (var cell in column.Cells.Skip(startRow).Take(height))
                {
                    cell.SetFormat(format);
                }
            }

            return this;
        }

        /// <summary> Проверяет и собирает таблицу </summary>
        public Table Build()
        {
            AlignAndCheck();
            return this;
        }

        /// <summary>
        /// Сериализовать в таблицу
        /// </summary>
        /// <param name="parameters">Имя таблицы</param>
        /// <param name="tableSerializer">Сериализатор</param>
        /// <typeparam name="T">Тип таблицы</typeparam>
        public T Serialize<T>(ITableSerializerParameters parameters, ITableSerializer<T> tableSerializer) =>
            tableSerializer.Serialize(Build(), parameters);

        private void AlignAndCheck()
        {
            var merges = new List<TableMergedArea>();

            foreach (var column in Columns)
            {
                var biggest = column.Cells.Max(x => x.Width);

                foreach (var x in column.Cells)
                {
                    if (x.Width < biggest)
                        x.SetWidth(biggest);

                    if (x.Format == null)
                        x.SetFormat(column.DefaultCellFormat);

                    if (!x.Merged || merges.Contains(x.Area))
                        continue;

                    SetMergeCells(x.Area);
                    merges.Add(x.Area);
                }
            }

            if (Width != null)
            {
                var wrongRow = Rows.FirstOrDefault(x => x.Width > Width);

                if (wrongRow != null)
                {
                    throw new Exception(
                        $"Длина строки {wrongRow.Number}: {wrongRow.Width} не соответствует ширине таблицы: {Width}");
                }
            }

            if (Rows.GroupBy(x => x.Width).Count() > 1)
                throw new Exception("Строки таблицы должы иметь одинаковую длину");
        }

        private void SetMergeCells(TableMergedArea area)
        {
            for (var r = area.TopRow; r <= area.BottomRow; r++)
            {
                for (var c = area.LeftColumn; c <= area.RightColumn; c++)
                    this[r, c].Area = area;
            }
        }

        private Cell FindByIndex(int rowNumber, int cellNumber)
        {
            if (rowNumber < 0 || cellNumber < 0)
                throw new IndexOutOfRangeException($"row: {rowNumber} cell: {cellNumber}");

            var row = Rows.FirstOrDefault(x => x.Number == rowNumber) ??
                      throw new IndexOutOfRangeException($"Строка {rowNumber} не существует");

            var cell = row.Cells.FirstOrDefault(x => x.Number == cellNumber) ??
                       throw new IndexOutOfRangeException($"Столбец {cellNumber} не существует");

            return cell;
        }
    }
}