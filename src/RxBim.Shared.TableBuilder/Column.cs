namespace RxBim.Shared.TableBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    /// <inheritdoc />
    public class Column : CellsSet<Column>
    {
        /// <inheritdoc />
        public Column(Table table)
            : base(table, table.Columns.Count)
        {
            DefaultCellFormat = new CellFormatStyle();
        }

        /// <summary> Форматирование колонки </summary>
        public CellFormatStyle DefaultCellFormat { get; private set; }

        /// <summary> Ширина колонки </summary>
        public double Width { get; private set; }

        /// <summary> Устанавливает ширину колонки </summary>
        /// <param name="width">Ширина в мм</param>
        public Column SetWidth(double width)
        {
            if (width <= 0)
                throw new ArgumentException($"Ширина строки: {width} не может быть меньше или равной 0", nameof(width));

            Width = width;
            return this;
        }

        /// <summary>
        /// Устанавливает формат по умолчанию
        /// </summary>
        /// <param name="format">Формат</param>
        public Column SetFormat(CellFormatStyle format)
        {
            DefaultCellFormat = format ?? throw new ArgumentException("Формат столбца должен быть задан");
            return this;
        }

        /// <inheritdoc />
        public override Column FromList<TSource>(
            IEnumerable<TSource> source,
            int row,
            int column,
            params Func<TSource, object>[] propertySelectors)
        {
            var list = source?.ToList();
            if (list?.Any() != true || propertySelectors?.Any() != true)
                return this;

            var matrix = new ICellData[propertySelectors.Length, list.Count];

            for (var r = 0; r < propertySelectors.Length; r++)
            {
                var prop = propertySelectors[r];
                for (var c = 0; c < list.Count; c++)
                {
                    var value = prop.Invoke(list[c]);
                    matrix[r, c] = new TextCellData(value.ToString());
                }
            }

            return FromMatrix(row, column, matrix);
        }
    }
}