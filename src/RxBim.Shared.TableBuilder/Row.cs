namespace RxBim.Shared.TableBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    /// <summary> Строка таблицы </summary>
    public class Row : CellsSet<Row>
    {
        /// <inheritdoc />
        public Row(Table table)
            : base(table, table.Rows.Count)
        {
        }

        /// <summary>
        /// Высота строки
        /// При билде каждая ячейка выравнивается по высоте строки
        /// </summary>
        public double Height { get; private set; }

        /// <summary> Ширина всех ячеек строки </summary>
        public double Width => Cells.Sum(x => x.Width);

        /// <summary>
        /// Устанавливает высоту строки в мм
        /// При билде каждая ячейка выравнивается по высоте строки
        /// </summary>
        /// <param name="height">Высота в мм</param>
        public Row SetHeight(double height)
        {
            if (height <= 0)
                throw new ArgumentException("Высота строки не может быть меньше или равной 0", nameof(height));

            Height = height;
            return this;
        }

        /// <inheritdoc />
        public override Row FromList<TSource>(
            IEnumerable<TSource> source,
            int row,
            int column,
            params Func<TSource, object>[] propertySelectors)
        {
            var list = source?.ToList();

            if (list?.Any() != true || propertySelectors?.Any() != true)
                return this;

            var matrix = new ICellData[list.Count, propertySelectors.Length];

            for (var c = 0; c < propertySelectors.Length; c++)
            {
                var prop = propertySelectors[c];
                for (var r = 0; r < list.Count; r++)
                {
                    var value = prop.Invoke(list[r]);
                    matrix[r, c] = new TextCellData(value.ToString());
                }
            }

            return FromMatrix(row, column, matrix);
        }
    }
}