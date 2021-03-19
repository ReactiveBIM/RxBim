namespace PikTools.Shared.TableBuilder
{
    using System.Drawing;

    /// <summary>
    /// Форматирование клетки
    /// </summary>
    public class CellFormatStyle
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="based">Основа</param>
        /// <param name="bold">Жирный</param>
        /// <param name="italic">Курсив</param>
        /// <param name="borders">Границы ячейки</param>
        /// <param name="textColor">Цвет текста</param>
        /// <param name="backgroundColor">Цвет фона</param>
        /// <param name="textSize">Размер текста</param>
        /// <param name="textHorizontalAlignment">Выравнивание по горизонтали</param>
        /// <param name="textVerticalAlignment">Выравнивание по вертикали</param>
        public CellFormatStyle(
            CellFormatStyle based = null,
            bool? bold = null,
            bool? italic = null,
            CellBorders borders = null,
            Color? textColor = null,
            Color? backgroundColor = null,
            int? textSize = null,
            TextHorizontalAlignment? textHorizontalAlignment = null,
            TextVerticalAlignment? textVerticalAlignment = null)
        {
            Bold = bold ?? based?.Bold ?? false;
            Italic = italic ?? based?.Italic ?? false;
            Borders = borders ?? based?.Borders ?? new CellBorders();
            TextColor = textColor ?? based?.TextColor ?? Color.Black;
            BackgroundColor = backgroundColor ?? based?.BackgroundColor ?? Color.White;
            TextSize = textSize ?? based?.TextSize ?? 0;
            TextHorizontalAlignment = textHorizontalAlignment ?? based?.TextHorizontalAlignment ?? TextHorizontalAlignment.Center;
            TextVerticalAlignment = textVerticalAlignment ?? based?.TextVerticalAlignment ?? TextVerticalAlignment.Bottom;
        }

        /// <summary> Жирный </summary>
        public bool Bold { get; set; }

        /// <summary> Курсив </summary>
        public bool Italic { get; set; }

        /// <summary> Размер текста </summary>
        public int TextSize { get; set; }

        /// <summary> Границы ячейки </summary>
        public CellBorders Borders { get; set; }

        /// <summary> Цвет </summary>
        public Color TextColor { get; set; }

        /// <summary> Цвет фона </summary>
        public Color BackgroundColor { get; set; }

        /// <summary> Выравнивание по горизонтали </summary>
        public TextHorizontalAlignment TextHorizontalAlignment { get; set; }

        /// <summary> Выравнивание по вертикали </summary>
        public TextVerticalAlignment TextVerticalAlignment { get; set; }
    }
}