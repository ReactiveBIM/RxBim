namespace RxBim.Shared.TableBuilder
{
    using Abstractions;

    /// <summary> Текстовое значение ячейки </summary>
    public class TextCellData : ICellData
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="value"> Значение </param>
        public TextCellData(string value)
        {
            Value = value;
        }

        /// <summary> Значение ячейки </summary>
        public string Value { get; set; }
    }
}