namespace RxBim.Shared.TableBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;

    /// <inheritdoc />
    public abstract class CellsSet<T> : TableBuilderBase
        where T : CellsSet<T>
    {
        /// <inheritdoc />
        protected CellsSet(Table table, int number)
            : base(table)
        {
            Number = number;
            Cells = new List<Cell>();
        }

        /// <summary> Номер в таблице </summary>
        public int Number { get; }

        /// <summary> Ячейки </summary>
        public List<Cell> Cells { get; }

        /// <summary>
        /// Заполняет набор на основе списка
        /// </summary>
        /// <param name="source">Список элементов</param>
        /// <param name="cellsAction">Делегат. Применяется для каждой вставленной ячейки</param>
        /// <typeparam name="TSource">Тип источника</typeparam>
        public T FromList<TSource>(List<TSource> source, Action<Cell> cellsAction = null)
        {
            if (source?.Any() != true)
                return this as T;

            if (source.Count > Cells.Count)
                throw new ArgumentException("Источник не должен быть длинее строки");

            for (var i = 0; i < source.Count; i++)
            {
                Cells[i].SetValue(new TextCellData(source[i].ToString()));
                cellsAction?.Invoke(Cells[i]);
            }

            return this as T;
        }

        /// <summary>
        /// Заполняет таблицу
        /// </summary>
        /// <param name="source">Источник</param>
        /// <param name="row">Строка</param>
        /// <param name="column">Колонка</param>
        /// <param name="propertySelectors">Селекторы свойств</param>
        /// <typeparam name="TSource">Тип источника</typeparam>
        public abstract T FromList<TSource>(
            IEnumerable<TSource> source,
            int row,
            int column,
            params Func<TSource, object>[] propertySelectors);

        /// <summary>
        /// Заполнение таблицы 2мерным массивом
        /// </summary>
        /// /// <param name="row">Стартовая строка</param>
        /// <param name="column">Стартовая колонка</param>
        /// <param name="matrix">2 мерный массив - источник</param>
        protected T FromMatrix(
            int row,
            int column,
            ICellData[,] matrix)
        {
            var diffRows = row + matrix.GetLength(0) - Table.Rows.Count;
            var diffCols = column + matrix.GetLength(1) - Table.Columns.Count;

            Table.AddRow(count: diffRows);
            Table.AddColumn(count: diffCols);

            for (var r = 0; r < matrix.GetLength(0); r++)
            {
                for (var c = 0; c < matrix.GetLength(1); c++) 
                    Table[row + r, column + c].SetValue(matrix[r, c]);
            }

            return this as T;
        }
    }
}