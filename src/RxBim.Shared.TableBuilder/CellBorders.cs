namespace RxBim.Shared.TableBuilder
{
    /// <summary>
    /// Границы ячейки
    /// </summary>
    public class CellBorders
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="top">Верхняя граница</param>
        /// <param name="bottom">Нижняя граница</param>
        /// <param name="left">Левая граница</param>
        /// <param name="right">Правая граница</param>
        public CellBorders(
            CellBorderType top = CellBorderType.Usual,
            CellBorderType bottom = CellBorderType.Usual,
            CellBorderType left = CellBorderType.Usual,
            CellBorderType right = CellBorderType.Usual)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Создает все 4 границы заданного типа
        /// </summary>
        /// <param name="type">Тип</param>
        public CellBorders(CellBorderType type)
        {
            Top = type;
            Bottom = type;
            Left = type;
            Right = type;
        }

        /// <summary> Верхняя </summary>
        public CellBorderType Top { get; set; }

        /// <summary> Нижняя </summary>
        public CellBorderType Bottom { get; set; }

        /// <summary> Левая </summary>
        public CellBorderType Left { get; set; }

        /// <summary> Правая </summary>
        public CellBorderType Right { get; set; }
    }
}