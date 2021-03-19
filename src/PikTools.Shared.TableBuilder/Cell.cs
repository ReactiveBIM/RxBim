﻿namespace PikTools.Shared.TableBuilder
{
    using System;
    using PikTools.Shared.TableBuilder.Abstractions;

    /// <summary>
    /// Ячейка таблицы
    /// </summary>
    public class Cell : TableBuilderBase
    {
        private readonly Row _row;
        private readonly Column _column;

        /// <inheritdoc />
        public Cell(Table table, Row row, Column column, TextCellData data = null, CellFormatStyle format = null)
            : base(table)
        {
            _row = row;
            _column = column;
            Data = data ?? new TextCellData(string.Empty);
            Format = format ?? new CellFormatStyle();
            Number = _row.Cells.Count;
        }

        private enum Rotation
        {
            Next,
            Down
        }

        /// <summary> Высота </summary>
        public double Height => _row.Height;

        /// <summary> Ширина </summary>
        public double Width => _column.Width;

        /// <summary> Номер в строке </summary>
        public int Number { get; }

        /// <summary> Объединена </summary>
        public bool Merged => Area != null;

        /// <summary>
        /// Поле объединенных клеток
        /// </summary>
        public TableMergedArea Area { get; set; }

        /// <summary> Значение </summary>
        public ICellData Data { get; private set; }

        /// <summary> Форматирование </summary>
        public CellFormatStyle Format { get; private set; }

        /// <summary>
        /// Устанавливает значение ячейки
        /// </summary>
        /// <param name="data">Значение ячейки</param>
        public Cell SetValue(ICellData data)
        {
            Data = data;
            return this;
        }

        /// <summary>
        /// Устанавливает ширину ячейки в мм
        /// </summary>
        /// <param name="width">Ширина в мм</param>
        public Cell SetWidth(double width)
        {
            _column.SetWidth(width);
            return this;
        }

        /// <summary>
        /// Задать формат ячейки
        /// </summary>
        /// <param name="format">Формат</param>
        public Cell SetFormat(CellFormatStyle format)
        {
            Format = format;
            return this;
        }

        /// <summary>
        /// Возвращает ячейку справа
        /// </summary>
        /// <param name="step">Шаг</param>
        public Cell Next(int step = 1) => Table[_row.Number, Number + step];

        /// <summary>
        /// Возвращает нижнюю ячейку
        /// </summary>
        /// <param name="step">Шаг</param>
        public Cell Down(int step = 1) => Table[_row.Number + step, Number];

        /// <summary>
        /// Возвращает ячейку слева
        /// </summary>
        /// <param name="step">Шаг</param>
        public Cell Previous(int step = 1) => Table[_row.Number, Number - step];

        /// <summary>
        /// Возвращает верхнюю ячейку
        /// </summary>
        /// <param name="step">Шаг</param>
        public Cell Up(int step = 1) => Table[_row.Number - step, Number];

        /// <summary>
        /// Объединяет клетки по горизонтали
        /// Возвращает контекст исходной ячейки
        /// </summary>
        /// <param name="count">Диапозон</param>
        /// <param name="action">Делегат, применяется к объединяемым клеткам</param>
        public Cell MergeDown(int count = 1, Action<Cell, Cell> action = null)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Шаг должен быть положительным числом");

            if (_row.Number + count > Table.Rows.Count)
                throw new ArgumentOutOfRangeException("Для объединения недостаточно строк в стобце");

            MergeInternal(count, Rotation.Down, action);
            return this;
        }

        /// <summary>
        /// Объединяет клетки по горизонтали
        /// </summary>
        /// <param name="count">Диапозон</param>
        /// <param name="action">Делегат, применяется к объединяемым клеткам</param>
        public Cell MergeNext(int count = 1, Action<Cell, Cell> action = null)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Шаг должен быть положительным числом");

            if (Number + count > _row.Cells.Count)
                throw new ArgumentOutOfRangeException("Для объединения недостаточно ячеек в столбце");

            MergeInternal(count, Rotation.Next, action);
            return this;
        }

        private void MergeInternal(int count, Rotation rotation, Action<Cell, Cell> action)
        {
            Area ??= new TableMergedArea(_row.Number, Number);

            for (var i = 1; i <= count; i++)
            {
                var next = rotation == Rotation.Next ? Next(i) : Down(i);

                if (next.Area != null)
                    throw new Exception("Среди объединяемых ячеек не должно быть уже объединенных");

                action?.Invoke(this, next);
                next.Data = Data;

                next.Area = rotation == Rotation.Next
                    ? Area.MergeRight()
                    : Area.MergeDown();
            }
        }
    }
}