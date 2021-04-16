namespace PikTools.Shared.TableBuilder
{
    /// <summary>
    /// Модель объединения клеток таблицы
    /// </summary>
    public class TableMergedArea
    {
        /// <summary> ctor </summary>
        /// <param name="row">Строка</param>
        /// <param name="col">Колонка</param>
        public TableMergedArea(int row, int col)
        {
            TopRow = row;
            BottomRow = row;

            LeftColumn = col;
            RightColumn = col;
        }

        /// <summary>
        /// Координата верхней строки
        /// </summary>
        public int TopRow { get; set; }

        /// <summary>
        /// Координата нижней строки
        /// </summary>
        public int BottomRow { get; set; }

        /// <summary>
        /// Координата левой колонки
        /// </summary>
        public int LeftColumn { get; set; }

        /// <summary>
        /// Координата правой колонки
        /// </summary>
        public int RightColumn { get; set; }

        /// <summary>
        /// Объединяет с клеткой справа
        /// </summary>
        public TableMergedArea MergeRight()
        {
            RightColumn++;
            return this;
        }

        /// <summary>
        /// Объединяет с клеткой справа
        /// </summary>
        public TableMergedArea MergeDown()
        {
            BottomRow++;
            return this;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is TableMergedArea other))
                return false;

            return TopRow == other.TopRow &&
                   BottomRow == other.BottomRow &&
                   LeftColumn == other.LeftColumn &&
                   RightColumn == other.RightColumn;
        }
    }
}